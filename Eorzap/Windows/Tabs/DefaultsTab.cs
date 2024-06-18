using Dalamud.Interface.Utility.Raii;
using ImGuiNET;
using OtterGui.Widgets;
using System;
using System.Numerics;

namespace Eorzap.Windows.Tabs
{
    public class DefaultsTab : ITab
    {
        public Configuration _config;
        public ReadOnlySpan<byte> Label => "Defaults"u8;
        public DefaultsTab(Configuration config)
        {
            _config = config;
        }

        public void DrawContent()
        {
            using (var child = ImRaii.Child("DefaultsChild"))
            {
                DrawDefaults();
            }
        }
        private void DrawDefaults()
        {
            var width = ImGui.GetContentRegionAvail().X;
            using var child = ImRaii.Child("##DefaultsPanel", new Vector2(width, -1), true, ImGuiWindowFlags.NoScrollbar);

            var spacing = ImGui.GetStyle().ItemInnerSpacing with { Y = ImGui.GetStyle().ItemInnerSpacing.Y };
            ImRaii.PushStyle(ImGuiStyleVar.ItemSpacing, spacing);

            ImGui.Spacing();
            if (_config.ApiKey.Length > 0)
            {
                ImGui.Text("Api Key is set");
                ImGui.Spacing();
            }
            if (_config.ShockUsername.Length > 0)
            {
                ImGui.Text("Username is set");
                ImGui.Spacing();
            }
            if (_config.ShockerCode.Length > 0)
            {
                ImGui.Text("Code is set");
                ImGui.Spacing();
            }
            ImGui.Text($"Last ApiCall answer: {_config.resultApiCall}");
        }
    }
}
