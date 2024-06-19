using ImGuiNET;
using OtterGui.Raii;
using OtterGui.Widgets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Eorzap.Windows.Tabs
{
    public class DefaultsTab : ITab
    {
        private Configuration _config;
        public ReadOnlySpan<byte> Label => "Defaults"u8;
        public DefaultsTab(Configuration config)
        {
            _config = config;
        }

        public void DrawContent()
        {
            var width = ImGui.GetContentRegionAvail().X;
            using var child = ImRaii.Child("##DefaultsPanel", new Vector2(width, -1), true, ImGuiWindowFlags.NoScrollbar);

            var spacing = ImGui.GetStyle().ItemInnerSpacing with { Y = ImGui.GetStyle().ItemInnerSpacing.Y };
            ImRaii.PushStyle(ImGuiStyleVar.ItemSpacing, spacing);

            DrawDefaults();
        }

        private void DrawDefaults()
        {
            bool deathMode = _config.DeathMode;
            if (ImGui.Checkbox("Death Mode", ref deathMode))
            {
                _config.DeathMode = deathMode;
                if (!deathMode)
                {
                    _config.DeathModeCount = 0;
                }
                _config.Save();
            }
            ImGui.NewLine();
        }
    }
}
