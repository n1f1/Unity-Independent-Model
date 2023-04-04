using UnityEngine;

namespace GameMenu
{
    [CreateAssetMenu(menuName = "Create MainMenuConfiguration", fileName = "MainMenuConfiguration", order = 0)]
    public class MainMenuConfiguration : ScriptableObject
    {
        [SerializeField] private MainMenuScreen _mainMenuScreenTemplate;
    
        public MainMenuScreen MainMenuScreenTemplate => _mainMenuScreenTemplate;
    }
}