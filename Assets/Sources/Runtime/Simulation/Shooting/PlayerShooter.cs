using System;
using Model;
using Model.Characters.Shooting;
using UnityEngine;
using Utility;

namespace Simulation.Shooting
{
    internal class PlayerShooter : MonoBehaviour, ISimulation<ICharacterShooter>
    {
        private LayerMask _layerMask;
        private ICharacterShooter _simulation;
        private Camera _camera;

        private void Awake() =>
            _layerMask = LayerMask.GetMask("AimPlane");

        public ISimulation<ICharacterShooter> Initialize(ICharacterShooter enemyPlayerPrediction)
        {
            _simulation = enemyPlayerPrediction ?? throw new ArgumentNullException();
            _camera = Camera.main;

            return this;
        }

        public void UpdateTime(float deltaTime)
        {
            ProcessAiming();
            ProcessShooting();
        }

        private void ProcessAiming()
        {
            Ray ray = _camera.ScreenPointToRay(UnityEngine.Input.mousePosition);

            if (UnityEngine.Physics.Raycast(ray, out RaycastHit hit, 999f, _layerMask))
                _simulation.AimAt(hit.point.Convert());
            else
                _simulation.StopAiming();
        }

        private void ProcessShooting()
        {
            if (UnityEngine.Input.GetMouseButton(0))
            {
                if (_simulation.CanShoot())
                    _simulation.Shoot();
            }
        }
    }
}