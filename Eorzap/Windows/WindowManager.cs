using Dalamud.Interface.Windowing;
using Dalamud.Plugin;
using System;

namespace Eorzap.Windows
{
    public class WindowManager : IDisposable
    {
        private WindowSystem windowSystem = new("Eorzap");
        private readonly DalamudPluginInterface pi;
        private MainWindow mainWindow;

        public WindowManager(DalamudPluginInterface pi, MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
            this.pi = pi;
            windowSystem.AddWindow(mainWindow);
            pi.UiBuilder.Draw += this.Draw;
            pi.UiBuilder.OpenMainUi += this.ToggleUi;
        }

        public void ToggleUi()
        {
            mainWindow.Toggle();
        }

        private void Draw()
        {
            this.windowSystem.Draw();
        }

        public void Dispose()
        {
            this.windowSystem.RemoveAllWindows();

            pi.UiBuilder.Draw -= this.Draw;
            pi.UiBuilder.OpenMainUi -= this.ToggleUi;

            mainWindow.Dispose();
        }
    }
}
