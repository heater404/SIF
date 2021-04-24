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
using StatusBar;
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
using ConfigCorrection.Views;
using ConfigCorrection;
using ConfigPostProc;
using ConfigPostProc.Views;

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

        protected override void OnStartup(StartupEventArgs e)
        {
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

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<ICommClient, SktClient>();
            containerRegistry.RegisterSingleton<ICommunication, Communication>();
            containerRegistry.RegisterSingleton<IInitArithParams, InitArithParams>();
            containerRegistry.RegisterSingleton<IInitCamera, InitCamera>();
            containerRegistry.RegisterSingleton<CaptureDataViewModel, CaptureDataViewModel>();
            containerRegistry.RegisterSingleton<ConfigCameraView, ConfigCameraView>();
            containerRegistry.RegisterSingleton<ConfigCorrectionView, ConfigCorrectionView>();
            containerRegistry.RegisterSingleton<ConfigPostProcView, ConfigPostProcView>();

            containerRegistry.RegisterDialog<CaptureDataView>(DialogNames.CaptureDataDialog);
            containerRegistry.RegisterDialog<WaitingView>(DialogNames.WaitingDialog);
            containerRegistry.RegisterDialog<NotificationView>(DialogNames.NotificationDialog);

            containerRegistry.RegisterForNavigation<PointCloudView>(ViewNames.PointCloudView);
            containerRegistry.RegisterForNavigation<RegMapView>(ViewNames.RegMapView);
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            //moduleCatalog.AddModule<MenuModule>();
            moduleCatalog.AddModule<ToolModule>();
            moduleCatalog.AddModule<WatchLogModule>();
            moduleCatalog.AddModule<ConfigCameraModule>();
            moduleCatalog.AddModule<ConfigCorrectionModule>();
            moduleCatalog.AddModule<ConfigPostProcModule>();
            moduleCatalog.AddModule<StatusBarModule>();
            moduleCatalog.AddModule<PointCloudModule>();
            moduleCatalog.AddModule<RegMapModule>();
        }
    }
}
