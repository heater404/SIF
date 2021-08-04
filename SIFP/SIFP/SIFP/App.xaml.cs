using Services;
using Services.Interfaces;
using Menu;
using Prism.Ioc;
using Prism.Modularity;
using Serilog;
using SIFP.Views;
using System.Text;
using System.Windows;
using Tool;
using BinarySerialization;
using System.IO;
using SIFP.Core.Models;
using WatchLog;
using ConfigCamera;
using CaptureDataDialog.Views;
using SIFP.Core;
using WaitingDialog.Views;
using PointCloud;
using NotificationDialog.Views;
using ConfigCamera.Views;
using PointCloud.Views;
using RegMap.Views;
using RegMap;
using CaptureDataDialog.ViewModels;
using System;
using License;
using VcselDriverDialog.Views;
using VcselDriverDialog.ViewModels;
using PasswordDialog.Views;
using System.Threading;
using ConifgAlg.Views;
using ConfigArithParams.Views;
using ConfigArithParams;

namespace SIFP
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override Window CreateShell()
        {
            if (VerityLicense.VerifyLicense("ToFDemo_License.lic", "ToF_2610_2021"))
                return Container.Resolve<MainWindow>();
            else
                return Container.Resolve<NoLicenseWindow>();
        }

        Mutex mutex = null;
        protected override void OnStartup(StartupEventArgs e)
        {
            mutex = new Mutex(true, "SI View", out bool createdNew);
            if (!createdNew)
            {
                ShowToFront();
                Environment.Exit(0);//强制退出，不会有弹框提示
            }

            base.OnStartup(e);
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}")
                .WriteTo.File("Logs/Log.txt", outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss}] [{Level:u3}] {Message:lj}{NewLine}{Exception}",
                             rollingInterval: RollingInterval.Day,//日志按日保存，这样会在文件名称后自动加上日期后缀
                             rollOnFileSizeLimit: true,          // 限制单个文件的最大长度    
                             encoding: Encoding.UTF8,            // 文件字符编码     
                             retainedFileCountLimit: 10,         // 最大保存文件数     
                             fileSizeLimitBytes: 10 * 1024)      // 最大单个文件长度
                .CreateLogger();
            //todo:configuration in App.config
        }

        private void ShowToFront()
        {
            //s1:通过WAPi:FindWindow获取运行实例的句柄
            //或者事先保存实例，传递过来
            IntPtr hwnd = Win32Api.FindWindow(null, "SI View");

            if (hwnd != IntPtr.Zero)
            {
                Win32Api.SetForegroundWindow(hwnd);
                if (Win32Api.IsIconic(hwnd))
                    Win32Api.OpenIcon(hwnd);
            }
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<ICommClient, SktClient>();
            containerRegistry.RegisterSingleton<ICommunication, Communication>();
            containerRegistry.RegisterSingleton<IInitArithParams, InitArithParams>();
            containerRegistry.RegisterSingleton<IInitCamera, InitCamera>();
            containerRegistry.RegisterSingleton<IInitConfigAlg, InitConfigAlg>();
            containerRegistry.RegisterSingleton<CaptureDataViewModel, CaptureDataViewModel>();
            containerRegistry.RegisterSingleton<ConfigCameraView, ConfigCameraView>();
            containerRegistry.RegisterSingleton<ConfigArithParamsView, ConfigArithParamsView>();
            containerRegistry.RegisterSingleton<ConfigAlgView, ConfigAlgView>();
            containerRegistry.RegisterSingleton<VcselDriverViewModel, VcselDriverViewModel>();
            containerRegistry.RegisterSingleton<RegMapServer, RegMapServer>();

            containerRegistry.RegisterInstance(typeof(int), 10000);
            containerRegistry.RegisterSingleton<HeartBeat, HeartBeat>();

            containerRegistry.RegisterDialog<CaptureDataView>(DialogNames.CaptureDataDialog);
            containerRegistry.RegisterDialog<WaitingView>(DialogNames.WaitingDialog);
            containerRegistry.RegisterDialog<NotificationView>(DialogNames.NotificationDialog);
            containerRegistry.RegisterDialog<VcselDriverView>(DialogNames.VcselDriverDialog);
            containerRegistry.RegisterDialog<PasswordView>(DialogNames.PasswordDialog);

            containerRegistry.RegisterForNavigation<PointCloudView>(ViewNames.PointCloudView);
            containerRegistry.RegisterForNavigation<RegMapView>(ViewNames.RegMapView);
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            //moduleCatalog.AddModule<MenuModule>();
            moduleCatalog.AddModule<ToolModule>();
            moduleCatalog.AddModule<WatchLogModule>();
            moduleCatalog.AddModule<ConfigCameraModule>();
            moduleCatalog.AddModule<ConfigArithParamsModule>();
            moduleCatalog.AddModule<StatusBarModule>();
            moduleCatalog.AddModule<PointCloudModule>();
            moduleCatalog.AddModule<RegMapModule>();
        }
    }
}
