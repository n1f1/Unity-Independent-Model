﻿using System;
using Model;
using Model.Characters.Shooting.Bullets;
using UnityEngine;

namespace Simulation.Shooting
{
    internal class BulletDestroyer : MonoBehaviour, ISimulation
    {
        private IBulletDestroyer _destroyer;
        private DefaultBullet _defaultBullet;

        public void Initialize(DefaultBullet defaultBullet, IBulletDestroyer destroyer)
        {
            _defaultBullet = defaultBullet ?? throw new ArgumentException();
            _destroyer = destroyer ?? throw new ArgumentException();
        }

        public void UpdatePassedTime(float deltaTime)
        {
            if (gameObject.activeSelf && _defaultBullet.ShouldBeDestroyed)
                _destroyer.Destroy(_defaultBullet);
        }
    }
}