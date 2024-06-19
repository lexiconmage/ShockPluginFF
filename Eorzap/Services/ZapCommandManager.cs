using Dalamud.Game.Command;
using Dalamud.Plugin.Services;
using Eorzap.Windows;

namespace Eorzap.Services
{
    public class ZapCommandService
    {
        private const string CommandName = "/eorzap";
        private readonly ICommandManager _commands;
        private readonly WindowManager _windowManager;

        public ZapCommandService(ICommandManager commands, WindowManager windows)
        {
            _commands = commands;
            _windowManager = windows;

            commands.AddHandler(CommandName, new CommandInfo(OnCommand)
            {
                HelpMessage = "Open Eorzap main window"
            });
        }

        private void OnCommand(string command, string args)
        {
            _windowManager.ToggleUi();
        }

        public void Dispose()
        {
            _commands.RemoveHandler(CommandName);
        }
    }
}
