﻿using System;
using SinglePlayer;
using UI;

namespace GameMenu
{
    public class PauseGame : ICommand
    {
        private readonly IPause _pause;

        public PauseGame(IPause pause)
        {
            _pause = pause ?? throw new ArgumentNullException(nameof(pause));
        }

        public void Execute()
        {
            _pause.Pause();
        }
    }
}