using Model.Characters.Shooting.Bullets;
using UnityEngine;
using Utility;

namespace GameModes.SinglePlayer.ObjectComposition.Bullets
{
    public class BulletTemplate : MonoBehaviour
    {
        [SerializeField] private MonoBehaviour _bulletSimulation;
        [SerializeField] private MonoBehaviour _bulletView;

        public IBulletView BulletView => (IBulletView) _bulletView;
        public IBulletSimulation BulletSimulation => (IBulletSimulation) _bulletSimulation;

        private void OnValidate()
        {
            InspectorInterfaceInjection.TrySetObject<IBulletSimulation>(ref _bulletSimulation);
            InspectorInterfaceInjection.TrySetObject<IBulletView>(ref _bulletView);
        }
    }
}