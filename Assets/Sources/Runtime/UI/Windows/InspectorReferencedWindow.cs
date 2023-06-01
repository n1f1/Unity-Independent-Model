using UnityEngine;
using Utility;

namespace UI.Windows
{
    public class InspectorReferencedWindow : MonoBehaviour, IWindow
    {
        [SerializeField] private MonoBehaviour _windowBehavior;

        private IWindow _window => (IWindow) _windowBehavior;

        private void OnValidate() =>
            InspectorInterfaceInjection.TrySetObject<IWindow>(ref _windowBehavior);

        public virtual void Open() =>
            _window.Open();

        public virtual void Close() =>
            _window.Close();
    }
}