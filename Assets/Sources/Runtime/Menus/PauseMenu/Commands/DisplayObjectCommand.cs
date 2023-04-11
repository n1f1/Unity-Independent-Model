using System;
using UI.Commands;
using UnityEngine;

namespace Menus.PauseMenu.Commands
{
    public class DisplayObjectCommand : ICommand
    {
        private readonly GameObject _gameObject;

        public DisplayObjectCommand(GameObject gameObject)
        {
            _gameObject = gameObject ? gameObject : throw new ArgumentNullException(nameof(gameObject));
        }

        public void Execute()
        {
            _gameObject.SetActive(true);
        }
    }
}