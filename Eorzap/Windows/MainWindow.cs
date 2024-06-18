using System;
using System.Numerics;
using Dalamud.Interface.Internal;
using Dalamud.Interface.Internal.Windows.Settings;
using Dalamud.Interface.Windowing;
using Eorzap.Windows.Tabs;
using FFXIVClientStructs.FFXIV.Common.Configuration;
using ImGuiNET;
using OtterGui.Widgets;

namespace Eorzap.Windows;

public enum TabType
{
    None = -1,
    Settings = 0,
    Defaults = 1,
    Triggers = 2
}

public class MainWindow : Window, IDisposable
{
    private readonly ITab[] _tabs;
    public readonly ConfigSettingsTab Settings;
    public readonly DefaultsTab Defaults;
    public readonly TriggersTab Triggers;
    private Configuration _config;

    public MainWindow(ConfigSettingsTab settings, DefaultsTab defaults, TriggersTab triggers, Configuration config) : base(
        "EorZap", ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse)
    {
        this.SizeConstraints = new WindowSizeConstraints
        {
            MinimumSize = new Vector2(375, 330),
            MaximumSize = new Vector2(float.MaxValue, float.MaxValue)
        };
        Settings = settings;
        Defaults = defaults;
        Triggers = triggers;
        _tabs =
        [
            settings,
            defaults,
            triggers
        ];
        _config = config;
    }

    public void Dispose()
    {
    }

    public override void Draw()
    {

        if (TabBar.Draw("##tabs", ImGuiTabBarFlags.None, _tabs))
        {

        }
        


    }
}
