using System;
using Model.Characters.CharacterHealth;

namespace GameModes.SinglePlayer
{
    public class SetLooseGameStatus : IDeathView
    {
        private readonly GameStatus _gameStatus;

        public SetLooseGameStatus(GameStatus gameStatus)
        {
            _gameStatus = gameStatus ?? throw new ArgumentNullException(nameof(gameStatus));
        }

        public void Die()
        {
            _gameStatus.Loose();
        }
    }
}