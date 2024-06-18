using Dalamud.Plugin;
using Dalamud.Plugin.Services;
using OtterGui.Classes;
using OtterGui.Services;

namespace Eorzap.Services
{
    internal class DalamudServices
    {
        public static void AddServices(ServiceManager services, DalamudPluginInterface pi)
        {
            services.AddExistingService(pi);
            services.AddExistingService(pi.UiBuilder);
            services.AddDalamudService<ICommandManager>(pi);
            services.AddDalamudService<IChatGui>(pi);
            services.AddDalamudService<IFramework>(pi);
        }
    }
}
