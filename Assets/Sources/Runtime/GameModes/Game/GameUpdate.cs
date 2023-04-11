using System;
using System.Collections.Generic;
using Model;
using UnityEngine;

namespace GameModes.Game
{
    public class GameUpdate
    {
        private readonly List<IUpdatable> _updatables = new();

        public void Update()
        {
            foreach (IUpdatable updatable in _updatables)
                updatable.UpdateTime(Time.deltaTime);
        }

        public void AddUpdate(IUpdatable updatable) =>
            _updatables.Add(updatable ?? throw new ArgumentNullException());
    }
}