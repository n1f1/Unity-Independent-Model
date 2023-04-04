using TMPro;
using UnityEngine;

namespace MultiPlayer.Connection
{
    internal class TextServerConnectionView : MonoBehaviour, IServerConnectionView
    {
        [SerializeField] private TextMeshProUGUI _textMeshPro;

        public void DisplayConnecting()
        {
            _textMeshPro.text = "Connecting...";
        }

        public void DisplayError(string errorMessage)
        {
            _textMeshPro.text = errorMessage;
        }

        public void DisplayConnected()
        {
            _textMeshPro.text = "Connected!";
        }
    }
}