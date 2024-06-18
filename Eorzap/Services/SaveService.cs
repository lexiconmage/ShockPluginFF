using OtterGui.Classes;
using OtterGui.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
