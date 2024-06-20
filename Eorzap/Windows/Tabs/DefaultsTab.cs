using Eorzap.Types;
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
                _config.Save();
            }
            ImGui.NewLine();
            if (deathMode)
            {
                var DeathModeSettings = _config.DeathModeSettings;
                if (ImGui.ListBox("Mode##dscale", ref DeathModeSettings[0], ["Shock", "Vibrate", "Beep"], 3))
                {
                    _config.DeathModeSettings = DeathModeSettings;
                    _config.Save();
                }
                ImGui.SliderInt("Max Intensity##dscaleInt", ref DeathModeSettings[1], 1, 100);
                ImGui.SliderInt("Max Duration##dscaleDur", ref DeathModeSettings[2], 1, 15);
                ImGui.Spacing();
                ImGui.Spacing();
            }
        }
    }
}
