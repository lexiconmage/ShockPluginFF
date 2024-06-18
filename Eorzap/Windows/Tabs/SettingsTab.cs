using System;
using ImGuiNET;
using OtterGui.Widgets;
using OtterGui.Raii;
using OtterGui;
using Dalamud.Interface.Utility;
using System.Numerics;

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
            if (ImGui.InputTextWithHint("##ApiKey", "ApiKey, found on your pishock profile", ref apiKey, 255))
            {
                _config.ApiKey = apiKey;
            }
            var username = _config.ShockUsername;
            if (ImGui.InputTextWithHint("##Username", "The username used on Pishock, case sensitive", ref username, 255))
            {
                _config.ShockUsername = username;
            }
            var shockCode = _config.ShockerCode;
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

            // can't ref a property, so use a local copy
            // Part for nomal chat
            var configValue = _config.Say;
            if (ImGui.Checkbox("Say", ref configValue))
            {
                _config.Say = configValue;
                _config.InfoChannel();
            }
            ImGui.SameLine();
            var configShout = _config.Shout;
            if (ImGui.Checkbox("Shout", ref configShout))
            {
                _config.Shout = configShout;
                _config.InfoChannel();
            }
            ImGui.SameLine();
            var configYell = _config.Yell;
            if (ImGui.Checkbox("Yell", ref configYell))
            {
                _config.Yell = configYell;
                _config.InfoChannel();
            }
            ImGui.SameLine();
            var configTell = _config.Tell;
            if (ImGui.Checkbox("Tell", ref configTell))
            {
                _config.Tell = configTell;
                _config.InfoChannel();
            }
            ImGui.Spacing();
            var configParty = _config.Party;
            if (ImGui.Checkbox("Party", ref configParty))
            {

                _config.Party = configParty;          
                _config.InfoChannel();
            }
            ImGui.SameLine();
            var configAlliance = _config.AllianceChat;
            if (ImGui.Checkbox("Alliance", ref configAlliance))
            {

                _config.AllianceChat = configAlliance;          
                _config.InfoChannel();
            }
            ImGui.SameLine();
            var configFc = _config.FcChat;
            if (ImGui.Checkbox("FC", ref configFc))
            {

                _config.FcChat = configFc;          
                _config.InfoChannel();
            }

            ImGui.Spacing();
            ImGui.NewLine();
            //Part for linkshell
            ImGui.Unindent(10);
            ImGui.Text("Linkshells :");
            ImGui.Indent(10);


            var configLs1 = _config.Ls1;
            if (ImGui.Checkbox("Linkshell1", ref configLs1))
            {
                _config.Ls1 = configLs1;
                _config.InfoChannel();
            }
            ImGui.SameLine();
            var configLs2 = _config.Ls2;
            if (ImGui.Checkbox("Linkshell2", ref configLs2))
            {
                _config.Ls2 = configLs2;
                _config.InfoChannel();
            }
            ImGui.SameLine();
            var configLs3 = _config.Ls3;
            if (ImGui.Checkbox("Linkshell3", ref configLs3))
            {
                _config.Ls3 = configLs3;
                // can save immediately on change, if you don't want to provide a "Save and Close" button
                _config.InfoChannel();
            }
            ImGui.SameLine();
            var configLs4 = _config.Ls4;
            if (ImGui.Checkbox("Linkshell4", ref configLs4))
            {
                _config.Ls4 = configLs4;
                // can save immediately on change, if you don't want to provide a "Save and Close" button
                _config.InfoChannel();
            }
            ImGui.Spacing();
            var configLs5 = _config.Ls5;
            if (ImGui.Checkbox("Linkshell5", ref configLs5))
            {
                _config.Ls5 = configLs5;
                _config.InfoChannel();
            }
            ImGui.SameLine();
            var configLs6 = _config.Ls6;
            if (ImGui.Checkbox("Linkshell6", ref configLs6))
            {
                _config.Ls6 = configLs6;
                _config.InfoChannel();
            }
            ImGui.SameLine();
            var configLs7 = _config.Ls7;
            if (ImGui.Checkbox("Linkshell7", ref configLs7))
            {
                _config.Ls7 = configLs7;
                _config.InfoChannel();
                _config.Save();
            }
            ImGui.SameLine();
            var configLs8 = _config.Ls8;
            if (ImGui.Checkbox("Linkshell8", ref configLs8))
            {
                _config.Ls8 = configLs8;
                // can save immediately on change, if you don't want to provide a "Save and Close" button
                _config.InfoChannel();
                _config.Save();
            }
            ImGui.NewLine();

            //Part for crossworldLinkShell
            ImGui.Unindent(10);
            ImGui.Text("CrossLinkshells :");
            ImGui.Indent(10);

            var configCLs1 = _config.CLs1;
            if (ImGui.Checkbox("Cls1", ref configCLs1))
            {
                _config.CLs1 = configCLs1;
                // can save immediately on change, if you don't want to provide a "Save and Close" button
                _config.InfoChannel();
                _config.Save();
            }
            ImGui.SameLine();
            var configCLs2 = _config.CLs2;
            if (ImGui.Checkbox("Cls2", ref configCLs2))
            {
                _config.CLs2 = configCLs2;
                // can save immediately on change, if you don't want to provide a "Save and Close" button
                _config.InfoChannel();
                _config.Save();
            }
            ImGui.SameLine();
            var configCLs3 = _config.CLs3;
            if (ImGui.Checkbox("Cls3", ref configCLs3))
            {
                _config.CLs3 = configCLs3;
                // can save immediately on change, if you don't want to provide a "Save and Close" button
                _config.InfoChannel();
                _config.Save();
            }
            ImGui.SameLine();
            var configCLs4 = _config.CLs4;
            if (ImGui.Checkbox("Cls4", ref configCLs4))
            {
                _config.CLs4 = configCLs4;
                // can save immediately on change, if you don't want to provide a "Save and Close" button
                _config.InfoChannel();
                _config.Save();
            }
            ImGui.Spacing();
            var configCLs5 = _config.CLs5;
            if (ImGui.Checkbox("Cls5", ref configCLs5))
            {
                _config.CLs5 = configCLs5;
                // can save immediately on change, if you don't want to provide a "Save and Close" button
                _config.InfoChannel();
                _config.Save();
            }
            ImGui.SameLine();
            var configCLs6 = _config.CLs6;
            if (ImGui.Checkbox("Cls6", ref configCLs6))
            {
                _config.CLs6 = configCLs6;
                // can save immediately on change, if you don't want to provide a "Save and Close" button
                _config.InfoChannel();
                _config.Save();
            }
            ImGui.SameLine();
            var configCLs7 = _config.CLs7;
            if (ImGui.Checkbox("Cls7", ref configCLs7))
            {
                _config.CLs7 = configCLs7;
                // can save immediately on change, if you don't want to provide a "Save and Close" button
                _config.InfoChannel();
                _config.Save();
            }
            ImGui.SameLine();
            var configCLs8 = _config.CLs8;
            if (ImGui.Checkbox("Cls8", ref configCLs8))
            {
                _config.CLs8 = configCLs8;
                // can save immediately on change, if you don't want to provide a "Save and Close" button
                _config.InfoChannel();
                _config.Save();
            }
            ImGui.NewLine();


            ImGui.Unindent(30);
            ImGui.EndGroup();

            ImGui.Spacing();
            ImGui.Text("Main Keyword to listen to:");
            ImGui.Spacing();
            var configKeyword = _config.mainKeyWord;
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
