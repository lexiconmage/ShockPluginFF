using Dalamud.Interface.Utility.Raii;
using ImGuiNET;
using OtterGui.Widgets;
using System;
using System.Numerics;

namespace Eorzap.Windows.Tabs
{
    public class InfoTab : ITab
    {
        public Configuration _config;
        public ReadOnlySpan<byte> Label => "Info"u8;
        public InfoTab(Configuration config)
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

            DrawApiConfig();
            DrawApiResults();
        }

        private void DrawApiConfig()
        {
            if (!ImGui.CollapsingHeader("API Configuration"))
            {
                return;
            }
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
            ImGui.NewLine();
        }
        private void DrawApiResults()
        {
            if (!ImGui.CollapsingHeader("API Results"))
            {
                return;
            }
            if (_config.resultApiCall.Length > 0)
            {
                ImGui.Text($"Last ApiCall answer: {_config.resultApiCall}");
            }
            else
            {
                ImGui.Text("The PiShock API has not been called yet.");
            }
        }
    }
}
