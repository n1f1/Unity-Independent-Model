using Model;
using UnityEngine;

namespace View
{
    public interface IViewFactory<out TView> where TView : IView
    {
        public TView Create(GameObject gameObject);
    }
}