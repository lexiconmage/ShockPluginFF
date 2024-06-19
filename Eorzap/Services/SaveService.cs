using OtterGui.Classes;
using OtterGui.Log;

namespace Eorzap.Services
{
    public interface ISavable : ISavable<FilenameService>
    {
    }
    public sealed class SaveService : SaveServiceBase<FilenameService>
    {
        public SaveService(Logger log, FrameworkManager framework, FilenameService fileNames)
            : base(log, framework, fileNames)
        {
        }
    }
}
