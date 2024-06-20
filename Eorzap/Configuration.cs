using Dalamud.Configuration;
using Dalamud.Game.Text;
using Dalamud.Game.Text.SeStringHandling;
using Dalamud.Interface.Internal.Notifications;
using Dalamud.Plugin;
using Eorzap.Services;
using Eorzap.Types;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace Eorzap
{
    [Serializable]
    public class Configuration : IPluginConfiguration, ISavable
    {
        public int Version { get; set; } = 0;
        public string ApiKey { get; set; } = string.Empty;
        public string ShockerCode { get; set; } = string.Empty;
        public string ShockUsername { get; set; } = string.Empty;
        public string resultApiCall { get; set; } = string.Empty;
        public bool DeathMode { get; set; } = false;
        public int[] DeathModeSettings { get; set; } = [0, 100, 15];
        public string MessageTest { get; set; } = "default";

        public List<ChatType.ChatTypes> Channels { get; set; }
        public List<Trigger> Triggers { get; set; }


        // the below exist just to make saving less cumbersome
        [JsonIgnore]
        private readonly SaveService _saveService;

        public Configuration(SaveService saveService)
        {
            _saveService = saveService;
            Load();
            if (Channels == null)
            {
                Channels = new List<ChatType.ChatTypes>();
            }
            if (Triggers == null)
            {
                Triggers = new List<Trigger>();
            }
        }

        public void Save()
        {
            _saveService.DelaySave(this);
        }

        public void Save(StreamWriter writer)
        {
            using var jWriter = new JsonTextWriter(writer) { Formatting = Formatting.Indented };
            var serializer = new JsonSerializer { Formatting = Formatting.Indented };
            serializer.Serialize(jWriter, this);
        }

        public string ToFilename(FilenameService fileNames)
        => fileNames.ConfigFile;

        public void Load()
        {
            if (!File.Exists(_saveService.FileNames.ConfigFile))
                return;
            if (File.Exists(_saveService.FileNames.ConfigFile))
            {
                try
                {
                    var text = File.ReadAllText(_saveService.FileNames.ConfigFile);
                    JsonConvert.PopulateObject(text, this);
                }
                catch (Exception ex)
                {
                    
                }
            }
        }
    }
}
