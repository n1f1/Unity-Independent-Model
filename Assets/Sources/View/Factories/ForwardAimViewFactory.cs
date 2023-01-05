using Model.Characters.Shooting;
using UnityEngine;

namespace View.Factories
{
    internal class ForwardAimViewFactory : IViewFactory<IForwardAimView>
    {
        public IForwardAimView Create(GameObject gameObject) => 
            gameObject.GetComponentInChildren<IForwardAimView>();
    }
}