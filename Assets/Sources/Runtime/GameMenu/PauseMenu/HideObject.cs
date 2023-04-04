using System;
using UI;
using UnityEngine;

namespace GameMenu
{
    public class HideObject : ICommand
    {
        private readonly GameObject _gameObject;

        public HideObject(GameObject gameObject)
        {
            _gameObject = gameObject ? gameObject : throw new ArgumentNullException(nameof(gameObject));
        }

        public void Execute()
        {
            _gameObject.SetActive(false);
        }
    }
}