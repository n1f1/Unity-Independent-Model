﻿using GameModes.MultiPlayer.PlayerCharacter.Remote;
using GameModes.SinglePlayer.ObjectComposition;
using GameModes.SinglePlayer.ObjectComposition.PlayerConstruction;
using Model;
using Simulation;

namespace GameModes.MultiPlayer.PlayerCharacter
{
    public interface IRemotePlayerSimulation : IPlayerSimulation
    {
        ISimulation<RemotePlayerPrediction> PlayerMovePrediction { get; set; }
    }
}