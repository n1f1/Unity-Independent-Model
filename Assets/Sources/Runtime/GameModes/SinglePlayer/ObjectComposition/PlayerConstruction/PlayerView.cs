﻿using GameModes.SinglePlayer.ObjectComposition.CharacterWithHealth;
using Model.Characters.CharacterHealth;
using Model.Characters.Player;
using Model.Characters.Shooting;
using Simulation.View;

namespace GameModes.SinglePlayer.ObjectComposition.PlayerConstruction
{
    public class PlayerView : CharacterWithHealthView, IPlayerView
    {
        public IForwardAimView ForwardAimView { get; set; }

        private void Awake()
        {
            //TODO: editor assertions
            PositionView = gameObject.AddComponent<PositionView>();
            HealthView = gameObject.GetComponentInChildren<IHealthView>();
            ForwardAimView = gameObject.GetComponentInChildren<IForwardAimView>();
        }
    }
}