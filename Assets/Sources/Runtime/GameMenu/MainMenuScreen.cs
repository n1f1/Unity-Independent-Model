using UI;
using UnityEngine;

namespace GameMenu
{
    public class MainMenuScreen : MonoBehaviour
    {
        [SerializeField] private CommandButton _singlePlayerButton;
        [SerializeField] private CommandButton _multiplayerPlayerButton;
        [SerializeField] private InspectorReferencedWindow _inspectorReferencedWindow;

        public CommandButton SinglePlayerButton => _singlePlayerButton;
        public CommandButton MultiplayerButton => _multiplayerPlayerButton;
        public IWindow Window => _inspectorReferencedWindow;
    }
}