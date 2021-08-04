using MaterialDesignThemes.Wpf;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;
using Serilog;
using Services;
using Services.Interfaces;
using SIFP.Core;
using SIFP.Core.Enums;
using SIFP.Core.Models;
using SIFP.Core.Mvvm;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Tool.ViewModels
{
    public class ToolViewModel : RegionViewModelBase
    {
        private bool isConnected = false;
        public bool IsConnected
        {
            get { return isConnected; }
            set
            {
                isConnected = value;
                RaisePropertyChanged();
                CanConnectCtrlCmd = CanConnectCtrl();
                CanStreamingCtrlCmd = CanStreamingOn();
                CanCaptureCtrlCmd = CanCaptureCtrl();

                if (!value)
                    this.EventAggregator.GetEvent<DisconnectCameraReplyEvent>().Publish(null);
            }
        }

        private bool isStreaming = false;
        public bool IsStreaming
        {
            get { return isStreaming; }
            set
            {
                isStreaming = value;
                RaisePropertyChanged();
                this.EventAggregator.GetEvent<IsStreamingEvent>().Publish(value);

                //是否能够捕获取决于是否streaming
                CanConnectCtrlCmd = CanConnectCtrl();

                CanStreamingCtrlCmd = CanStreamingOn();

                CanCaptureCtrlCmd = CanCaptureCtrl();
            }
        }

        private bool isCapturing = false;
        public bool IsCapturing
        {
            get { return isCapturing; }
            set
            {
                isCapturing = value;
                RaisePropertyChanged();

                //Capturing时不允许断开连接
                CanConnectCtrlCmd = CanConnectCtrl();

                CanStreamingCtrlCmd = CanStreamingOn();

                CanCaptureCtrlCmd = CanCaptureCtrl();
            }
        }
        private bool CanCaptureCtrl()
        {
            return isStreaming;
        }
        private bool CanConnectCtrl()
        {
            return !isCapturing;
        }
        private bool CanStreamingOn()
        {
            if (isCapturing)
                return false;
            else
                return IsConnected & isConfigCameraSuccess;
        }
        private bool isConfigCameraSuccess = false;
        public bool IsConfigCameraSuccess
        {
            get { return isConfigCameraSuccess; }
            set
            {
                isConfigCameraSuccess = value;

                CanConnectCtrlCmd = CanConnectCtrl();

                CanStreamingCtrlCmd = CanStreamingOn();

                CanCaptureCtrlCmd = CanCaptureCtrl();
            }
        }

        private bool canConnectCtrlCmd = true;
        public bool CanConnectCtrlCmd
        {
            get { return canConnectCtrlCmd; }
            set { canConnectCtrlCmd = value; RaisePropertyChanged(); }
        }

        private bool canStreamingCtrlCmd;
        public bool CanStreamingCtrlCmd
        {
            get { return canStreamingCtrlCmd; }
            set { canStreamingCtrlCmd = value; RaisePropertyChanged(); }
        }

        private bool canCatureCtrlCmd;
        public bool CanCaptureCtrlCmd
        {
            get { return canCatureCtrlCmd; }
            set { canCatureCtrlCmd = value; RaisePropertyChanged(); }
        }

        private bool isDebug;
        public DelegateCommand ConnectCtrlCmd { get; private set; }
        public DelegateCommand StreamingCtrlCmd { get; private set; }
        public DelegateCommand VcselDriverShowCmd { get; set; }
        public DelegateCommand CaptureDataShowCmd { get; private set; }
        private IDialogService dialogService;
        private ICommunication comm;
        private Process processor = null;

        private CancellationTokenSource cancellationTokenSource = null;
        private LensCaliArgs lensArgs = new LensCaliArgs();
        private Size resolution = new Size();
        private UserAccessType user = UserAccessType.Normal;
        AutoResetEvent lensArgsHandle = new AutoResetEvent(false);
        public ToolViewModel(HeartBeat beat, ICommunication comm, IDialogService dialogService, IRegionManager regionManager, IEventAggregator eventAggregator) : base(regionManager, eventAggregator)
        {
            this.comm = comm;
            this.dialogService = dialogService;
            VcselDriverShowCmd = new DelegateCommand(ShowVcselDriverDialog).ObservesCanExecute(() => IsConnected);
            CaptureDataShowCmd = new DelegateCommand(ShowCaptureDialog).ObservesCanExecute(() => CanCaptureCtrlCmd);
            ConnectCtrlCmd = new DelegateCommand(ConnectCtrl).ObservesCanExecute(() => CanConnectCtrlCmd);
            StreamingCtrlCmd = new DelegateCommand(StreamingCtrl).ObservesCanExecute(() => CanStreamingCtrlCmd);
            EventAggregator.GetEvent<ConfigCameraReplyEvent>().Subscribe(RecvConfigCameraReply, true);
            EventAggregator.GetEvent<CloseAppEvent>().Subscribe(() =>
            {
                if (isStreaming)
                {
                    comm.StopStreaming(beat.Alive ? 5000 : 0);
                    EventAggregator.GetEvent<ClosePointCloudEvent>().Publish();
                }

                if (isConnected)
                {
                    comm.DisconnectCamera(beat.Alive ? 3000 : 0);
                    KillAssembly(processor);
                }
            }, ThreadOption.PublisherThread, true);//CLose窗体的时候触发
            EventAggregator.GetEvent<CaptureReplyEvent>().Subscribe(reply =>
            {
                IsCapturing = false;
            }, true);

            this.EventAggregator.GetEvent<IsDebugEvent>().Subscribe(arg => isDebug = arg, true);

            this.EventAggregator.GetEvent<ConfigCameraSuccessEvent>().Subscribe(isSuccess =>
            {
                IsConfigCameraSuccess = isSuccess;
            }, ThreadOption.PublisherThread, true);

            this.EventAggregator.GetEvent<UserAccessChangedEvent>().Subscribe(type =>
            {
                user = type;
            }, true);

            this.EventAggregator.GetEvent<LensArgsReplyEvent>().Subscribe(reply =>
            {
                this.lensArgs = reply.LensArgs;
                lensArgsHandle.Set();
            }, true);

            this.EventAggregator.GetEvent<ConfigVcselDriverReplyEvent>().Subscribe(reply =>
            {
                this.reply = reply;
            }, true);
        }

        private ConfigVcselDriverReply reply;
        private void ShowVcselDriverDialog()
        {
            IDialogParameters parameters = new DialogParameters();
            parameters.Add("ConfigVcselDriverReply", reply);
            dialogService.Show(DialogNames.VcselDriverDialog, parameters, null);
        }

        private void RecvConfigCameraReply(ConfigCameraReply reply)
        {
            resolution = new Size(reply.OutImageWidth, reply.OutImageHeight - reply.AddInfoLines);
        }

        private void ShowCaptureDialog()
        {
            if (!isCapturing)
                dialogService.ShowDialog(DialogNames.CaptureDataDialog, CaptureCallback);
            else
            {
                var param = new DialogParameters
                {
                    { "notice", "Capturing....... Are you sure to stop???" }
                };
                dialogService.ShowDialog(DialogNames.NotificationDialog, param, result =>
                 {
                     if (result.Result == ButtonResult.Yes)
                     {
                         comm.AlgoDelCapture((UInt32)CapturePosition.Pos, (UInt32)CaptureID.ID);
                         IsCapturing = false;
                     }
                     else
                     {
                         IsCapturing = true;
                     }
                 });
            }
        }

        private void CaptureCallback(IDialogResult result)
        {
            if (result.Result == ButtonResult.OK)
            {
                IsCapturing = true;
            }
            else
            {
                IsCapturing = false;
            }
        }

        private async void StreamingCtrl()
        {
            dialogService.Show(DialogNames.WaitingDialog);
            if (!isStreaming)
            {
                if (await Task.Run(() => StreamingOn()))
                {

                    IsStreaming = true;
                }
                else
                {
                    IsStreaming = false;
                }
            }
            else
            {
                if (await Task.Run(() => StreamingOff()))
                {

                    IsStreaming = false;
                }
                else
                {
                    IsStreaming = true;
                }
            }
            EventAggregator.GetEvent<CloseWaitingDialogEvent>().Publish();
        }


        private bool StreamingOn()
        {
            var res = comm.StartStreaming(2000);
            if (res.HasValue)
            {
                if (!res.Value)
                {
                    this.PrintNoticeLog("StreamingOn Fail", LogLevel.Error);
                    this.PrintWatchLog("StreamingOn Fail", LogLevel.Error);
                    return false;
                }
                else
                {
                    this.PrintNoticeLog("StreamingOn Success", LogLevel.Warning);
                    this.PrintWatchLog("StreamingOn Success", LogLevel.Warning);
                }
            }
            else
            {
                this.PrintNoticeLog("StreamingOn Timeout", LogLevel.Error);
                this.PrintWatchLog("StreamingOn Timeout", LogLevel.Error);
                return false;
            }

            GetLenArgs();
            lensArgsHandle.WaitOne(200);
            this.PrintWatchLog(this.lensArgs.ToString(), LogLevel.Info);
            string args = resolution.Width + "*" + resolution.Height + "_" + this.lensArgs.ToString();
            EventAggregator.GetEvent<OpenPointCloudEvent>().Publish(args);
            return true;
        }

        private bool GetLenArgs()
        {
            var res = comm.GetLensArgs(5000);
            if (res.HasValue)
            {
                if (!res.Value)
                {
                    this.PrintWatchLog("GetLensArgs Fail", LogLevel.Error);
                }
                else
                {
                    this.PrintWatchLog("GetLensArgs Success", LogLevel.Warning);
                }
            }
            else
            {
                this.PrintWatchLog("GetLensArgs Timeout", LogLevel.Error);
            }

            return true;
        }

        private bool StreamingOff()
        {
            var res = comm.StopStreaming(5000);
            if (res.HasValue)
            {
                if (!res.Value)
                {
                    this.PrintNoticeLog("StreamingOff Fail", LogLevel.Error);
                    this.PrintWatchLog("StreamingOff Fail", LogLevel.Error);
                }
                else
                {
                    this.PrintNoticeLog("StreamingOff Success", LogLevel.Warning);
                    this.PrintWatchLog("StreamingOff Success", LogLevel.Warning);
                }
            }
            else
            {
                this.PrintNoticeLog("StreamingOff Timeout", LogLevel.Error);
                this.PrintWatchLog("StreamingOff Timeout", LogLevel.Error);
            }

            EventAggregator.GetEvent<ClosePointCloudEvent>().Publish();
            return true;
        }

        private async void ConnectCtrl()
        {
            dialogService.Show(DialogNames.WaitingDialog);
            if (!isConnected)
            {
                if (await Task.Run(ConnectCamera))
                {
                    comm.SwitchUserAccess(user);

                    cancellationTokenSource = new CancellationTokenSource();
                    comm.GetSysStatusAsync(cancellationTokenSource.Token, 3000);

                    //有这个顺序要求
                    this.EventAggregator.GetEvent<ConfigAlgRequestEvent>().Publish();

                    this.EventAggregator.GetEvent<ConfigArithParamsRequestEvent>().Publish();

                    this.EventAggregator.GetEvent<ConfigCameraRequestEvent>().Publish();

                    IsConnected = true;
                }
                else
                {
                    if (await Task.Run(DisconnectCamera))
                        IsConnected = false;
                }
            }
            else
            {
                if (await Task.Run(DisconnectCamera))
                {
                    IsConnected = false;
                }
                else
                {
                    IsConnected = true;
                }
            }
            EventAggregator.GetEvent<CloseWaitingDialogEvent>().Publish();
        }

        private bool ConnectCamera()
        {
            processor = LaunchAssembly(@"PowerDataProcessor\PowerDataProcessor.exe", out AssemblyExitCode code);
            if (AssemblyExitCode.Success != code)
            {
                this.PrintNoticeLog(code.ToString(), LogLevel.Error);
                this.PrintWatchLog(code.ToString(), LogLevel.Error);
                return false;
            }

            if (!comm.Open())
            {
                this.PrintNoticeLog("Open CommClient Fail", LogLevel.Error);
                this.PrintWatchLog("Open CommClient Fail", LogLevel.Error);
                return false;
            }

            if (!comm.Detect(8))
            {
                this.PrintNoticeLog("DetectFail", LogLevel.Error);
                this.PrintWatchLog("DetectFail", LogLevel.Error);
                return false;
            }

            comm.Subscribe();//detect之后再subscribe

            var res = comm.ConnectCamera(3000);
            if (res.HasValue)
            {
                if (!res.Value)
                {
                    this.PrintNoticeLog("ConnectCamera Fail", LogLevel.Error);
                    this.PrintWatchLog("ConnectCamera Fail", LogLevel.Error);
                    return false;
                }
            }
            else
            {
                this.PrintNoticeLog("ConnectCamera Timeout", LogLevel.Error);
                this.PrintWatchLog("ConnectCamera Timeout", LogLevel.Error);
                return false;
            }

            this.PrintNoticeLog("ConnectCamera Success", LogLevel.Warning);
            this.PrintWatchLog("ConnectCamera Success", LogLevel.Warning);
            return true;
        }

        private Process LaunchAssembly(string path, out AssemblyExitCode code)
        {
            code = AssemblyExitCode.Success;

            Process[] pros = Process.GetProcessesByName(Path.GetFileNameWithoutExtension(path));
            if (0 < pros.Length)
            {
                if (1 == pros.Length)
                {
                    code = AssemblyExitCode.Success;
                    return pros[0];
                }
            }

            if (!File.Exists(path))
            {
                code = AssemblyExitCode.Assembly_Not_Exist;
                return null;
            }

            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = path;
            startInfo.UseShellExecute = false;
            if (!isDebug)
                startInfo.CreateNoWindow = true;

            Process pro = Process.Start(startInfo);
            if (null != pro)
            {
                pro.WaitForExit(500);//必须wait
                if (pro.HasExited)
                    code = (AssemblyExitCode)pro.ExitCode;
            }
            return pro;
        }

        private bool KillAssembly(Process pro)
        {
            try
            {
                if (null == pro)
                    return true;

                if (pro.HasExited)
                    return true;

                if (pro.CloseMainWindow())
                    if (pro.WaitForExit(300))
                        return true;

                if (!pro.HasExited)
                    pro.Kill(true);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "KillAssembly Error");
                return false;
            }

            return true;
        }

        private bool DisconnectCamera()
        {
            cancellationTokenSource?.Cancel();

            if (isStreaming)
            {
                if (!StreamingOff())
                    return false;
                else
                    IsStreaming = false;
            }

            if (isConnected)
            {
                var res = comm.DisconnectCamera(3000);
                if (res.HasValue)
                {
                    if (!res.Value)
                    {
                        this.PrintNoticeLog("DisonnectCamera Fail", LogLevel.Error);
                        this.PrintWatchLog("DisonnectCamera Fail", LogLevel.Error);
                    }
                    else
                    {
                        this.PrintNoticeLog("DisconnectCamera Success", LogLevel.Warning);
                        this.PrintWatchLog("DisconnectCamera Success", LogLevel.Warning);
                    }
                }
                else
                {
                    this.PrintNoticeLog("DisonnectCamera Timeout", LogLevel.Error);
                    this.PrintWatchLog("DisonnectCamera Timeout", LogLevel.Error);
                }

                if (!comm.Close())
                {
                    this.PrintNoticeLog("Close CommClient Fail", LogLevel.Error);
                    this.PrintWatchLog("Close CommClient Fail", LogLevel.Error);
                }
            }

            KillAssembly(processor);

            Thread.Sleep(2000);
            return true;
        }
    }
}
