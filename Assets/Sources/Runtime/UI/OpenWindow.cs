using System;

namespace UI
{
    public class OpenWindow : ICommand
    {
        private readonly IWindow _mainMenu;

        public OpenWindow(IWindow mainMenu)
        {
            _mainMenu = mainMenu ?? throw new ArgumentNullException(nameof(mainMenu));
        }

        public void Execute() => 
            _mainMenu.Open();
    }
}