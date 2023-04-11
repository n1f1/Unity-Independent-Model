using Menus.PauseMenu;

namespace GameModes.GameStatus.Pause
{
    public class GamePause : IPause, IPauseStatus
    {
        public bool Paused { get; private set; }

        public void Pause()
        {
            Paused = true;
        }

        public void Unpause()
        {
            Paused = false;
        }
    }
}