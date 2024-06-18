using System;
using ImGuiNET;
using OtterGui.Widgets;
using OtterGui.Raii;
using OtterGui;
using Dalamud.Interface.Utility;
using System.Numerics;
using FFXIVClientStructs.FFXIV.Client.UI.Agent;
using Eorzap.Types;

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

            ImGui.Text("Api info:");
            ImGui.Spacing();
            ImGui.Indent(30);
            var apiKey = _config.ApiKey;
            ImGui.SetNextItemWidth(ImGuiHelpers.GlobalScale * 500);
            if (ImGui.InputTextWithHint("##ApiKey", "ApiKey, found on your pishock profile", ref apiKey, 255))
            {
                _config.ApiKey = apiKey;
            }
            var username = _config.ShockUsername;
            ImGui.SetNextItemWidth(ImGuiHelpers.GlobalScale * 500);
            if (ImGui.InputTextWithHint("##Username", "The username used on Pishock, case sensitive", ref username, 255))
            {
                _config.ShockUsername = username;
            }
            var shockCode = _config.ShockerCode;
            ImGui.SetNextItemWidth(ImGuiHelpers.GlobalScale * 500);
            if (ImGui.InputTextWithHint("##Code", "Code for the shocker you want controlled, can be generated on the pishock page", ref shockCode, 255))
            {
                _config.ShockerCode = shockCode;
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
                if (i != 0 && (i == 4 || i == 7 || i == 11 || i == 15 || i == 19))
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

            ImGui.Spacing();
            ImGui.Text("Main Keyword to listen to:");
            ImGui.Spacing();

            var configKeyword = _config.mainKeyWord;
            ImGui.SetNextItemWidth(ImGuiHelpers.GlobalScale * 230);
            if (ImGui.InputText("Main Keyword", ref configKeyword, 255))
            {
                _config.mainKeyWord = configKeyword;
                _config.Save();
            }

        }

        /// <summary>
        /// Check that the value of the duration doesnt 
        /// exceed the max Apishock value
        /// </summary>
        /// <param name="duration"> duration of a shock in seconds</param>
        /// <returns>Duration of a shock in seconds</returns>
        public int checkDuration(int duration)
        {
            if (duration < 0)
            {
                return duration = 0;
            }
            else if (duration > 15)
            {
                return duration = 15;
            }
            return duration;
        }


        /// <summary>
        /// Check that the intensity isnt over the
        /// api shock limite
        /// </summary>
        /// <param name="intensity">intensity of a shock</param>
        /// <returns>Intensity of a shock inside the apishocks limits</returns>
        public int checkIntensity(int intensity)
        {
            if (intensity < 0)
            {
                return intensity = 0;
            }
            else if (intensity > 100)
            {
                return intensity = 100;
            }
            return intensity;
        }
    }
}
