using MaterialDesignThemes.Wpf;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;
using Serilog;
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
            }
        }

        public DelegateCommand ConnectCtrlCmd { get; private set; }
        public DelegateCommand StreamingCtrlCmd { get; private set; }
        public DelegateCommand VcselDriverShowCmd { get; set; }
        public DelegateCommand CaptureDataShowCmd { get; private set; }
        private IDialogService dialogService;
        private ICommunication comm;
        private IStateMachine machine;
        private Process processor = null;

        private CancellationTokenSource cancellationTokenSource = null;
        private LensCaliArgs lensArgs = new LensCaliArgs();
        private Size resolution = new Size();
        private UserAccessType user = UserAccessType.Normal;
        public ToolViewModel(IStateMachine stateMachine, ICommunication comm, IDialogService dialogService, IRegionManager regionManager, IEventAggregator eventAggregator) : base(regionManager, eventAggregator)
        {
            this.machine = stateMachine;

            this.machine.ConfigureDisconnectAction(() => DisconnectCamera(), () => Debug.WriteLine("Exit Disconnect"));
            this.machine.ConfigureConnectedState(ConnectCamera, () => Debug.WriteLine("Exit Connect"));

            this.comm = comm;
            this.dialogService = dialogService;


            VcselDriverShowCmd = new DelegateCommand(ShowVcselDriverDialog);
            CaptureDataShowCmd = new DelegateCommand(ShowCaptureDialog);
            StreamingCtrlCmd = new DelegateCommand(StreamingCtrl);

            ConnectCtrlCmd = new DelegateCommand(ConnectCtrl, CanConnectCtrl);

            EventAggregator.GetEvent<ConfigCameraReplyEvent>().Subscribe(RecvConfigCameraReply, true);
            EventAggregator.GetEvent<DisconnectCameraRequestEvent>().Subscribe(() =>
            {
                if (isStreaming)
                {
                    comm.StopStreaming(0);
                    EventAggregator.GetEvent<ClosePointCloudEvent>().Publish();
                }

                if (isConnected)
                {
                    KillAssembly(processor);
                    comm.DisconnectCamera(0);
                }
            }, true);//CLose窗体的时候触发
            EventAggregator.GetEvent<CaptureReplyEvent>().Subscribe(reply =>
            {
                IsCapturing = false;
            }, true);

            this.EventAggregator.GetEvent<ConfigCameraSuccessEvent>().Subscribe(isSuccess =>
            {

            }, ThreadOption.PublisherThread, true);

            this.EventAggregator.GetEvent<UserAccessChangedEvent>().Subscribe(type =>
            {
                user = type;
            }, true);
        }

        private bool CanConnectCtrl()
        {
            if (isConnected)
                return machine.CanDisconnect();
            else
                return machine.CanConnect();
        }

        private void ShowVcselDriverDialog()
        {
            dialogService.Show(DialogNames.VcselDriverDialog);
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

            string args = resolution.Width + "*" + resolution.Height + "_" + this.lensArgs.ToString();
            EventAggregator.GetEvent<OpenPointCloudEvent>().Publish(args);
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
                this.machine.Connect();

                //if (await Task.Run(ConnectCamera))
                //{
                //    comm.SwitchUserAccess(user);

                //    cancellationTokenSource = new CancellationTokenSource();
                //    comm.GetSysStatusAsync(cancellationTokenSource.Token, 3000);

                //    //有这个顺序要求
                //    this.EventAggregator.GetEvent<ConfigAlgRequestEvent>().Publish();

                //    this.EventAggregator.GetEvent<ConfigArithParamsRequestEvent>().Publish();

                //    this.EventAggregator.GetEvent<ConfigCameraRequestEvent>().Publish();

                //    IsConnected = true;
                //}
                //else
                //{
                //    if (await Task.Run(DisconnectCamera))
                //        IsConnected = false;
                //}
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
                this.PrintWatchLog("Open Commlient Fail", LogLevel.Error);
                return false;
            }

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
                this.PrintNoticeLog("OpenCamera Timeout", LogLevel.Error);
                this.PrintWatchLog("OpenCamera Timeout", LogLevel.Error);
                return false;
            }

            this.PrintNoticeLog("OpenCamera Success", LogLevel.Warning);
            this.PrintWatchLog("OpenCamera Success", LogLevel.Warning);
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
