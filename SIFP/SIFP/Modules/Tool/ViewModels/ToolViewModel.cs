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

namespace Tool.ViewModels
{
    public class ToolViewModel : RegionViewModelBase
    {
        public DelegateCommand ConnectCtrlCmd { get; private set; }
        public DelegateCommand StreamingCtrlCmd { get; private set; }

        public DelegateCommand CaptureDataShowCmd { get; private set; }
        private IDialogService dialogService;
        private ICommunication comm;
        private Process processor = null;
        public ToolViewModel(ICommunication comm, IDialogService dialogService, IRegionManager regionManager, IEventAggregator eventAggregator) : base(regionManager, eventAggregator)
        {
            this.comm = comm;
            this.dialogService = dialogService;
            CaptureDataShowCmd = new DelegateCommand(CaptureDataShow);
            ConnectCtrlCmd = new DelegateCommand(ConnectCtrl);
            StreamingCtrlCmd = new DelegateCommand(StreamingCtrl);
            EventAggregator.GetEvent<ConnectCameraReplyEvent>().Subscribe(RecvConnectCameraReply);
        }

        private void CaptureDataShow()
        {
            dialogService.ShowDialog(DialogNames.CaptureDataDialog);
        }

        private void StreamingCtrl()
        {

        }

        private void RecvConnectCameraReply(ConnectCameraReply reply)
        {

        }

        private async void ConnectCtrl()
        {
            
            if (!isConnected)
            {
                dialogService.Show(DialogNames.WaitingDialog);
                if (await Task.Run(ConnectCamera))
                {
                    this.PrintNoticeLog("ConnectCamera Success", LogLevel.Warning);
                    this.PrintWatchLog("ConnectCamera Success", LogLevel.Warning);
                    IsConnected = true;
                }
                else
                {
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
        }
        private bool isConnected = false;
        public bool IsConnected
        {
            get { return isConnected; }
            set { isConnected = value; RaisePropertyChanged(); }
        }

        private bool isStreaming=false;
        public bool IsStreaming
        {
            get { return isStreaming; }
            set { isStreaming = value; RaisePropertyChanged(); }
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
            startInfo.WindowStyle = ProcessWindowStyle.Normal;

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
