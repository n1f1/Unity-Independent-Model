using Model;
using UnityEngine;

namespace View
{
    internal class ForwardAimViewFactory : IViewFactory<IForwardAimView>
    {
        public IForwardAimView Create(GameObject gameObject) => 
            gameObject.GetComponentInChildren<IForwardAimView>();
    }
}