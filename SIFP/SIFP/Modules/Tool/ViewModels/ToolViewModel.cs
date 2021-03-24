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
            set { isConnected = value; RaisePropertyChanged(); }
        }

        private bool isStreaming = false;
        public bool IsStreaming
        {
            get { return isStreaming; }
            set { isStreaming = value; RaisePropertyChanged(); }
        }

        private bool isCapturing = false;
        public bool IsCapturing
        {
            get { return isCapturing; }
            set { isCapturing = value; RaisePropertyChanged(); }
        }

        private bool canConnectCtrlCmd = true;
        public bool CanConnectCtrlCmd
        {
            get { return canConnectCtrlCmd; }
            set { canConnectCtrlCmd = value; RaisePropertyChanged(); }
        }

        private bool canStreamingCtrlCmd = false;
        public bool CanStreamingCtrlCmd
        {
            get { return canStreamingCtrlCmd; }
            set { canStreamingCtrlCmd = value; RaisePropertyChanged(); }
        }

        private bool canCaptureCtrlCmd = false;
        public bool CanCaptureCtrlCmd
        {
            get { return canCaptureCtrlCmd; }
            set { canCaptureCtrlCmd = value; RaisePropertyChanged(); }
        }


        public DelegateCommand ConnectCtrlCmd { get; private set; }
        public DelegateCommand StreamingCtrlCmd { get; private set; }

        public DelegateCommand CaptureDataShowCmd { get; private set; }
        private IDialogService dialogService;
        private ICommunication comm;
        private Process processor = null;
        private LensCaliArgs lensArgs = new LensCaliArgs();
        private Size resolution = new Size();
        public ToolViewModel(ICommunication comm, IDialogService dialogService, IRegionManager regionManager, IEventAggregator eventAggregator) : base(regionManager, eventAggregator)
        {
            this.comm = comm;
            this.dialogService = dialogService;
            CaptureDataShowCmd = new DelegateCommand(ShowCaptureDialog).ObservesCanExecute(() => CanCaptureCtrlCmd);
            ConnectCtrlCmd = new DelegateCommand(ConnectCtrl).ObservesCanExecute(() => CanConnectCtrlCmd);
            StreamingCtrlCmd = new DelegateCommand(StreamingCtrl).ObservesCanExecute(() => CanStreamingCtrlCmd);
            EventAggregator.GetEvent<ConfigCameraReplyEvent>().Subscribe(RecvConfigCameraReply);
            EventAggregator.GetEvent<DisconnectCameraRequestEvent>().Subscribe(() =>
            {
                if (isConnected)
                    Task.Run(() => DisconnectCamera()).Wait();
            });
            EventAggregator.GetEvent<CaptureReplyEvent>().Subscribe(reply =>
            {
                IsCapturing = false;
                CanCaptureCtrlCmd = true;
                CanStreamingCtrlCmd = true;
                CanConnectCtrlCmd = true;
            });
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
                var res = MessageBox.Show("It is capturing......Are you sure to stop capture？？？", "Notice", MessageBoxButton.YesNo);
                if (res == MessageBoxResult.Yes)
                {
                    comm.AlgoDelCapture((UInt32)CapturePosition.Pos, (UInt32)CaptureID.ID);

                    CanCaptureCtrlCmd = true;
                    CanStreamingCtrlCmd = true;
                    CanConnectCtrlCmd = true;
                    IsCapturing = false;
                }
                else
                    IsCapturing = true;
            }
        }

        private void CaptureCallback(IDialogResult result)
        {
            if (result.Result == ButtonResult.OK)
            {
                CanCaptureCtrlCmd = true;
                CanStreamingCtrlCmd = false;
                CanConnectCtrlCmd = false;
                IsCapturing = true;
            }
            else
            {
                CanCaptureCtrlCmd = true;
                CanStreamingCtrlCmd = true;
                CanConnectCtrlCmd = true;
                IsCapturing = false;
            }
        }

        private async void StreamingCtrl()
        {
            CanCaptureCtrlCmd = false;
            CanStreamingCtrlCmd = false;
            CanConnectCtrlCmd = false;
            dialogService.Show(DialogNames.WaitingDialog, result =>
            {
                CanCaptureCtrlCmd = true;
                CanStreamingCtrlCmd = true;
                CanConnectCtrlCmd = true;
            });
            if (!isStreaming)
            {
                if (await Task.Run(() => StreamingOn()))
                {
                    this.PrintNoticeLog("StreamingOn Success", LogLevel.Warning);
                    this.PrintWatchLog("StreamingOn Success", LogLevel.Warning);
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
                    this.PrintNoticeLog("StreamingOff Success", LogLevel.Warning);
                    this.PrintWatchLog("StreamingOff Success", LogLevel.Warning);
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
                    return false;
                }
            }
            else
            {
                this.PrintNoticeLog("StreamingOff Timeout", LogLevel.Error);
                this.PrintWatchLog("StreamingOff Timeout", LogLevel.Error);
                return false;
            }

            EventAggregator.GetEvent<ClosePointCloudEvent>().Publish();
            return true;
        }

        private async void ConnectCtrl()
        {
            CanCaptureCtrlCmd = false;
            CanStreamingCtrlCmd = false;
            CanConnectCtrlCmd = false;
            dialogService.Show(DialogNames.WaitingDialog, result =>
            {
                CanCaptureCtrlCmd = false;

                CanConnectCtrlCmd = true;
            });
            if (!isConnected)
            {
                if (await Task.Run(ConnectCamera))
                {
                    this.PrintNoticeLog("ConnectCamera Success", LogLevel.Warning);
                    this.PrintWatchLog("ConnectCamera Success", LogLevel.Warning);

                    comm.ConfigAlg(new ConfigAlgRequest
                    {
                        ByPassSocAlgorithm = true,
                        ReturnRawData = true,
                        ReturnAmplitudeImage = false,
                        ReturnBGImage = false,
                        ReturnConfidence = false,
                        ReturnDepthIamge = false,
                        ReturnFlagMap = false,
                        ReturnGrayImage = false,
                        ReturnPointcloud = false,
                    }, 3000);

                    this.EventAggregator.GetEvent<ConfigCameraRequestEvent>().Publish();

                    IsConnected = true;
                    CanStreamingCtrlCmd = true;
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
                    this.PrintNoticeLog("DisconnectCamera Success", LogLevel.Warning);
                    this.PrintWatchLog("DisconnectCamera Success", LogLevel.Warning);
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

            ProcessStartInfo startInfo = new ProcessStartInfo(path);
            startInfo.UseShellExecute = true;
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;

            Process pro = Process.Start(startInfo);
            pro.PriorityClass = ProcessPriorityClass.AboveNormal;

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
                    if (pro.WaitForExit(100))
                        return true;

                if (!pro.HasExited)
                    pro.Kill();
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
            if (isStreaming)
            {
                if (!StreamingOff())
                    return false;
                else
                    IsStreaming = false;
            }

            var res = comm.DisconnectCamera(3000);
            if (res.HasValue)
            {
                if (!res.Value)
                {
                    this.PrintNoticeLog("DisonnectCamera Fail", LogLevel.Error);
                    this.PrintWatchLog("DisonnectCamera Fail", LogLevel.Error);
                    return false;
                }
            }
            else
            {
                this.PrintNoticeLog("DisonnectCamera Timeout", LogLevel.Error);
                this.PrintWatchLog("DisonnectCamera Timeout", LogLevel.Error);
                return false;
            }

            if (!comm.Close())
            {
                this.PrintNoticeLog("Close CommClient Fail", LogLevel.Error);
                this.PrintWatchLog("Close Commlient Fail", LogLevel.Error);
                return false;
            }

            if (!KillAssembly(processor))
                return false;

            return true;
        }
    }
}
