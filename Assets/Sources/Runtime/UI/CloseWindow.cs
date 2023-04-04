using System;

namespace UI
{
    public class CloseWindow : ICommand
    {
        private readonly IWindow _mainMenu;

        public CloseWindow(IWindow mainMenu)
        {
            _mainMenu = mainMenu ?? throw new ArgumentNullException(nameof(mainMenu));
        }

        public void Execute() => 
            _mainMenu.Close();
    }
}