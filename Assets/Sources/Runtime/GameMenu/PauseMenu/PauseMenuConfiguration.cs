﻿using UnityEngine;

namespace GameMenu
{
    [CreateAssetMenu(menuName = "Create PauseMenuConfiguration", fileName = "PauseMenuConfiguration", order = 0)]
    public class PauseMenuConfiguration : ScriptableObject
    {
        [SerializeField] private PauseMenuScreen _pauseScreenTemplate;
    
        public PauseMenuScreen PauseScreenTemplate => _pauseScreenTemplate;
    }
}