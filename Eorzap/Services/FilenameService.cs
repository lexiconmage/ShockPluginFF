using Dalamud.Plugin;
using System.IO;

namespace Eorzap.Services
{
    public class FilenameService
    {
        public readonly string ConfigDirectory;
        public readonly string ConfigFile;
        public FilenameService(DalamudPluginInterface pi)
        {
            ConfigDirectory = pi.ConfigDirectory.FullName;
            ConfigFile = pi.ConfigFile.FullName;
        }
    }
}
