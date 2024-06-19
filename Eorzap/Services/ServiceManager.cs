using Dalamud.Plugin;
using Dalamud.Plugin.Services;
using Eorzap.Windows;
using Eorzap.Windows.Tabs;
using OtterGui.Classes;
using OtterGui.Log;
using OtterGui.Services;

namespace Eorzap.Services
{
    public static class StaticServiceManager
    {
        public static OtterGui.Services.ServiceManager CreateProvider(DalamudPluginInterface pi, Logger log)
        {
            var services = new ServiceManager(log)
                .AddUi()
                .AddLogic();
            DalamudServices.AddServices(services, pi);
            services.CreateProvider();
            return services;
        }

        public static OtterGui.Services.ServiceManager AddUi(this OtterGui.Services.ServiceManager services)
            => services.AddSingleton<WindowManager>()
                .AddSingleton<MainWindow>()
                .AddSingleton<InfoTab>()
                .AddSingleton<ConfigSettingsTab>()
                .AddSingleton<TriggersTab>()
                .AddSingleton<DefaultsTab>();

        public static OtterGui.Services.ServiceManager AddLogic(this OtterGui.Services.ServiceManager services)
            => services.AddSingleton<ZapCommandService>()
                .AddSingleton<ChatService>()
                .AddSingleton<Configuration>()
                .AddSingleton<FilenameService>()
                .AddSingleton<SaveService>()
                .AddSingleton<FrameworkManager>();

    }
}
