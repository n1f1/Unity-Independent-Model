using Model.Characters.CharacterHealth;
using UnityEngine;

namespace View.Factories
{
    internal class HealthViewFactory : IViewFactory<IHealthView>
    {
        public IHealthView Create(GameObject gameObject) =>
            gameObject.GetComponentInChildren<HealthBarView>();
    }
}