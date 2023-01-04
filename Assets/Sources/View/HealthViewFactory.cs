using Model;
using UnityEngine;

namespace View
{
    internal class HealthViewFactory : IViewFactory<IHealthView>
    {
        public IHealthView Create(GameObject gameObject) => 
            gameObject.GetComponentInChildren<HealthBarView>();
    }
}