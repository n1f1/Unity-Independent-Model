using Model;
using UnityEngine;
using Utility;

namespace Simulation
{
    internal class PlayerShooter : MonoBehaviour, ISimulation<CharacterShooter>
    {
        private LayerMask _layerMask;
        private CharacterShooter _simulation;
        private Camera _camera;

        private void Awake() =>
            _layerMask = LayerMask.GetMask("AimPlane");

        public ISimulation<CharacterShooter> Initialize(CharacterShooter simulation)
        {
            _simulation = simulation;
            _camera = Camera.main;

            return this;
        }

        public void UpdatePassedTime(float deltaTime)
        {
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
            
            if (Physics.Raycast(ray, out RaycastHit hit, 999f, _layerMask))
                _simulation.Aim(hit.point.Convert());
            else
                _simulation.StopAiming();

            if(Input.GetMouseButtonDown(0))
                _simulation.Shoot();
        }
    }
}