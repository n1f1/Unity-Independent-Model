using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    [RequireComponent(typeof(Button))]
    public class CommandButton : MonoBehaviour
    {
        private Button _button;
        private ICommand _command;

        public void Construct(ICommand command) => 
            _command = command ?? throw new ArgumentNullException(nameof(command));

        private void Awake() => 
            _button = GetComponent<Button>();

        private void OnEnable() => 
            _button.onClick.AddListener(Click);

        private void OnDisable() => 
            _button.onClick.RemoveListener(Click);

        private void Click()
        {
            _command.Execute();
            OnDisable();
        }
    }
}