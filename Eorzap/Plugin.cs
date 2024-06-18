
using Dalamud.Plugin;
using Eorzap.Windows;
using OtterGui.Services;
using OtterGui.Log;
using Eorzap.Services;

namespace Eorzap
{
    public sealed class Plugin : IDalamudPlugin
    {
        public string Name => "Eorzap";

        public static readonly Logger Log = new();

        private readonly ServiceManager _services;

        public Plugin(DalamudPluginInterface pluginInterface)
        {
            try
            {
                _services = StaticServiceManager.CreateProvider(pluginInterface, Log);
                _services.EnsureRequiredServices();
                _services.GetService<WindowManager>();
                _services.GetService<ZapCommandService>();
            }
            catch
            {
                Dispose();
                throw;
            }
        }

        public void Dispose()
        {
            _services.Dispose();
        }

    }
}
