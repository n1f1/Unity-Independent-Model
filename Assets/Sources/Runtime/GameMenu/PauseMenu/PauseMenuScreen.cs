using UI;
using UnityEngine;

namespace GameMenu.PauseMenu
{
    public class PauseMenuScreen : MonoBehaviour
    {
        [SerializeField] private CommandButton _pauseButton;
        [SerializeField] private CommandButton _resumeGame;
        [SerializeField] private CommandButton _mainMenu;
        [SerializeField] private FadeWindow _fadeWindow;
        
        public CommandButton PauseButton => _pauseButton;
        public CommandButton ResumeGame => _resumeGame;
        public CommandButton MainMenu => _mainMenu;
        public IWindow Window => _fadeWindow;

        private void Awake()
        {
            _fadeWindow.SetClosed(true);
        }
    }
}