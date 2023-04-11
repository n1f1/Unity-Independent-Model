using System;
using Model.Characters.Player;
using UnityEngine;
using Utility;

namespace Simulation.Characters.Player
{
    internal class PlayerShooter : MonoBehaviour, ISimulation<ICharacterShooter>
    {
        private LayerMask _layerMask;
        private ICharacterShooter _simulation;
        private Camera _camera;
        private RaycastHit[] _raycastHitBuffer;

        private void Awake() =>
            _layerMask = LayerMask.GetMask("AimPlane");

        public ISimulation<ICharacterShooter> Initialize(ICharacterShooter enemyPlayerPrediction)
        {
            _simulation = enemyPlayerPrediction ?? throw new ArgumentNullException();
            _camera = Camera.main;
            _raycastHitBuffer = new RaycastHit[1];

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

            if (UnityEngine.Physics.RaycastNonAlloc(ray, _raycastHitBuffer, 999f, _layerMask) > 0)
                _simulation.AimAt(_raycastHitBuffer[0].point.Convert());
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