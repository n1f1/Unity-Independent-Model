using System;
using System.Collections.Generic;
using Simulation.Input;
using UnityEngine;

namespace GameModes.MultiPlayer.PlayerCharacter.Client.Movement
{
    public class BotInput : IMovementInput
    {
        private readonly List<Vector2> _botMoveData;
        private int _x;
        private int _y;

        public BotInput(List<Vector2> botMoveData)
        {
            _botMoveData = botMoveData ?? throw new ArgumentNullException(nameof(botMoveData));
        }

        public float X => _botMoveData[_x++ % _botMoveData.Count].x;
        public float Y => _botMoveData[_y++ % _botMoveData.Count].y;
    }
}