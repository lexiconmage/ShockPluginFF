using Dalamud.Interface;
using Dalamud.Interface.Colors;
using Dalamud.Interface.Utility;
using Dalamud.Utility;
using Eorzap.Types;
using ImGuiNET;
using OtterGui;
using OtterGui.Raii;
using OtterGui.Widgets;
using OtterGuiInternal.Enums;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text.RegularExpressions;

namespace Eorzap.Windows.Tabs
{
    public class TriggersTab : ITab
    {
        public ReadOnlySpan<byte> Label => "Triggers"u8;
        private Configuration _config;

        public TriggersTab(Configuration config)
        {
            _config = config;
        }

        public void DrawContent()
        {
            var width = ImGui.GetContentRegionAvail().X;
            using var child = ImRaii.Child("##TriggersPanel", new Vector2(width, -1), true, ImGuiWindowFlags.NoScrollbar);

            var spacing = ImGui.GetStyle().ItemInnerSpacing with { Y = ImGui.GetStyle().ItemInnerSpacing.Y };
            ImRaii.PushStyle(ImGuiStyleVar.ItemSpacing, spacing);

            if (ImGuiUtil.DrawDisabledButton(FontAwesomeIcon.Plus.ToIconString(), ImGui.GetFrameHeight() * Vector2.One, "Add an empty trigger.", false, true))
            {
                _config.Triggers.Add(new());
                _config.Save();
            }

            DrawTriggers(_config.Triggers);
        }

        private void DrawTriggers(List<Trigger> triggers)
        {
            int cnt = 6;
            if(ImGui.BeginTable("##Triggers", cnt, ImGuiTableFlags.Borders | ImGuiTableFlags.Resizable | ImGuiTableFlags.Reorderable))
            {
                ImGui.TableSetupColumn(" ", ImGuiTableColumnFlags.NoResize | ImGuiTableColumnFlags.WidthFixed | ImGuiTableColumnFlags.NoSort);
                ImGui.TableSetupColumn("Name", ImGuiTableColumnFlags.None, ImGuiHelpers.GlobalScale * 100);
                ImGui.TableSetupColumn("Regex");
                ImGui.TableSetupColumn("Duration", ImGuiTableColumnFlags.None, ImGuiHelpers.GlobalScale * 100);
                ImGui.TableSetupColumn("Intensity", ImGuiTableColumnFlags.None, ImGuiHelpers.GlobalScale * 100);
                ImGui.TableSetupColumn(" ", ImGuiTableColumnFlags.NoResize | ImGuiTableColumnFlags.WidthFixed | ImGuiTableColumnFlags.NoSort);
                ImGui.TableHeadersRow();

                for(int i = 0; i < triggers.Count; i++)
                {
                    var trigger = triggers[i];

                    ImGui.PushID(trigger.GUID.ToString());

                    ImGui.TableNextColumn();
                    if (ImGui.Checkbox("##enabled", ref trigger.Enabled))
                    {
                        _config.Save();
                    }
                    if (ImGui.IsItemHovered())
                    {
                        ImGui.SetTooltip("Enable the trigger to be used.");
                    }

                    ImGui.TableNextColumn();
                    ImGui.SetNextItemWidth(ImGui.GetColumnWidth());
                    if(ImGui.InputTextWithHint("##name", "Trigger name", ref trigger.Name, 100))
                    {
                        _config.Save();
                    }

                    ImGui.TableNextColumn();
                    if (trigger.Regex == null)
                    {
                        ImGui.PushFont(UiBuilder.IconFont);
                        ImGui.TextColored(ImGuiColors.DPSRed, FontAwesomeIcon.ExclamationTriangle.ToIconString());
                        ImGui.PopFont();
                        if (ImGui.IsItemHovered())
                        {
                            ImGui.SetTooltip("Not a valid regex. Will not be parsed.");
                        }
                        ImGui.SameLine();
                    }
                    ImGui.SetNextItemWidth(ImGui.GetColumnWidth());
                    if (ImGui.InputTextWithHint("##regex", "Regex", ref trigger.RegexString, 200))
                    {
                        try
                        {
                            trigger.Regex = new Regex(trigger.RegexString);
                        }
                        catch(ArgumentException ex)
                        {
                            trigger.Regex = null;
                        }
                        _config.Save();
                    }

                    ImGui.TableNextColumn();
                    ImGui.SetNextItemWidth(ImGui.GetColumnWidth());
                    if (ImGui.InputInt("##duration", ref trigger.Duration, 1, 5))
                    {
                        trigger.Duration = checkDuration(trigger.Duration);
                        _config.Save();
                    }
                    if (ImGui.IsItemHovered())
                    {
                        ImGui.SetTooltip("1-15");
                    }

                    ImGui.TableNextColumn();
                    ImGui.SetNextItemWidth(ImGui.GetColumnWidth());
                    if (ImGui.InputInt("##intensity", ref trigger.Intensity, 1, 5))
                    {
                        trigger.Intensity = checkIntensity(trigger.Intensity);
                        _config.Save();
                    }
                    if (ImGui.IsItemHovered())
                    {
                        ImGui.SetTooltip("1-100");
                    }

                    ImGui.TableNextColumn();
                    if (ImGuiUtil.DrawDisabledButton(FontAwesomeIcon.Trash.ToIconString(), ImGui.GetFrameHeight() * Vector2.One, "Delete this trigger.", false, true))
                    {
                        _config.Triggers.Remove(trigger);
                        _config.Save();
                    }
                }
                ImGui.EndTable();
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
            if (duration < 1)
            {
                return duration = 1;
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
            if (intensity < 1)
            {
                return intensity = 1;
            }
            else if (intensity > 100)
            {
                return intensity = 100;
            }
            return intensity;
        }
    }
}
