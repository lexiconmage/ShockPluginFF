using System;
using ImGuiNET;
using OtterGui.Widgets;
using OtterGui.Raii;
using OtterGui;
using Dalamud.Interface.Utility;
using System.Numerics;
using FFXIVClientStructs.FFXIV.Client.UI.Agent;
using Eorzap.Types;
using System.Linq;

namespace Eorzap.Windows.Tabs
{
    public class ConfigSettingsTab : ITab
    {
        private Configuration _config;
        public ReadOnlySpan<byte> Label => "Settings"u8;

        public ConfigSettingsTab(Configuration config)
        {
            _config = config;
        }

        public void DrawContent()
        {
            using (var child = ImRaii.Child("SettingsChild"))
            {
                DrawConfigSettings();
            }
        }

        private void DrawConfigSettings()
        {
            var width = ImGui.GetContentRegionAvail().X;
            using var child = ImRaii.Child("##SettingsPanel", new Vector2(width, -1), true, ImGuiWindowFlags.NoScrollbar);

            var spacing = ImGui.GetStyle().ItemInnerSpacing with { Y = ImGui.GetStyle().ItemInnerSpacing.Y };
            ImRaii.PushStyle(ImGuiStyleVar.ItemSpacing, spacing);

            ImGui.Text("Api Info:");
            ImGui.Spacing();
            ImGui.Indent(30);
            var apiKey = _config.ApiKey;
            ImGui.SetNextItemWidth(ImGuiHelpers.GlobalScale * 300);
            if (ImGui.InputTextWithHint("##ApiKey", "PiShock Api Key", ref apiKey, 255))
            {
                _config.ApiKey = apiKey;
                _config.Save();
            }
            if (ImGui.IsItemHovered())
            {
                ImGui.SetTooltip("This can be found on your PiShock profile.");
            }

            var username = _config.ShockUsername;
            ImGui.SetNextItemWidth(ImGuiHelpers.GlobalScale * 300);
            if (ImGui.InputTextWithHint("##Username", "PiShock Username", ref username, 255))
            {
                _config.ShockUsername = username;
                _config.Save();
            }
            if (ImGui.IsItemHovered())
            {
                ImGui.SetTooltip("The username is case sensitive.");
            }

            var shockCode = _config.ShockerCode;
            ImGui.SetNextItemWidth(ImGuiHelpers.GlobalScale * 300);
            if (ImGui.InputTextWithHint("##Code", "PiShock Shocker Code", ref shockCode, 255))
            {
                _config.ShockerCode = shockCode;
                _config.Save();
            }
            if (ImGui.IsItemHovered())
            {
                ImGui.SetTooltip("Code for the specific shocker. This can be generated on the PiShock website.");
            }
            ImGui.Spacing();
            ImGui.Unindent(30);

            //Part for the listener

            ImGui.BeginGroup();
            ImGui.Text("Listen to the trigger on :");
            ImGui.Indent(30);

            var i = 0;
            foreach (var e in ChatType.GetOrderedChannels())
            {
                // See if it is already enabled by default
                var enabled = _config.Channels.Contains(e);
                // Create a new line after every 4 columns
                if (i != 0 && (i == 4 || i == 7 || i == 11 || i == 15 || i == 19 || i == 23))
                {
                    ImGui.NewLine();
                    //i = 0;
                }
                // Move to the next row if it is LS1 or CWLS1
                if (e is ChatType.ChatTypes.LS1 or ChatType.ChatTypes.CWL1)
                    ImGui.Separator();

                if (ImGui.Checkbox($"{e}", ref enabled))
                {
                    // See If the UIHelpers.Checkbox is clicked, If not, add to the list of enabled channels, otherwise, remove it.
                    if (enabled) _config.Channels.Add(e);
                    else _config.Channels.Remove(e);
                    _config.Save();
                }

                ImGui.SameLine();
                i++;
            }

            ImGui.Unindent(30);
            ImGui.EndGroup();
        }

    }
}
