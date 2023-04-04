using System;
using UI;
using UnityEngine;

namespace GameMenu
{
    public class DisplayObject : ICommand
    {
        private readonly GameObject _gameObject;

        public DisplayObject(GameObject gameObject)
        {
            _gameObject = gameObject ? gameObject : throw new ArgumentNullException(nameof(gameObject));
        }

        public void Execute()
        {
            _gameObject.SetActive(true);
        }
    }
}