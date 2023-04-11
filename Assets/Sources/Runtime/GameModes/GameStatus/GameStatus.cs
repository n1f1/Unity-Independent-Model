using System;
using GameModes.GameStatus.Pause;

namespace GameModes.GameStatus
{
    public class GameStatus : IPauseStatus
    {
        private readonly IPauseStatus _gamePause;

        public GameStatus(IPauseStatus gamePause)
        {
            _gamePause = gamePause ?? throw new ArgumentNullException(nameof(gamePause));
        }

        public bool Lost { get; private set; }
        public bool Finished { get; private set; }
        public bool Paused => _gamePause.Paused;

        public void Loose()
        {
            Lost = true;
            Finished = true;
        }
    }
}