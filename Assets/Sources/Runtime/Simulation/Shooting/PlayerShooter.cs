using System;
using Model;
using Model.Characters.Shooting;
using UnityEngine;
using Utility;

namespace Simulation.Shooting
{
    internal class PlayerShooter : MonoBehaviour, ISimulation<CharacterShooter>
    {
        private LayerMask _layerMask;
        private CharacterShooter _simulation;
        private Camera _camera;

        private void Awake() =>
            _layerMask = LayerMask.GetMask("AimPlane");

        public void Initialize(CharacterShooter simulation)
        {
            _simulation = simulation ?? throw new ArgumentException();
            _camera = Camera.main;
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
                _simulation.Aim(hit.point.Convert());
            else
                _simulation.StopAiming();
        }

        private void ProcessShooting()
        {
            if (UnityEngine.Input.GetMouseButton(0))
                _simulation.Shoot();
        }
    }
}