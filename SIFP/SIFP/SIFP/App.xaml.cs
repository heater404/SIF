using Menu;
using Prism.Ioc;
using Prism.Modularity;
using Serilog;
using SIFP.Services;
using SIFP.Services.Interfaces;
using SIFP.Views;
using System.Text;
using System.Windows;
using Tool;

namespace SIFP
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
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
            containerRegistry.RegisterSingleton<ICommunicationService, CommunicationService>();
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            moduleCatalog.AddModule<MenuModule>();
            moduleCatalog.AddModule<ToolModule>();
        }
    }
}
