﻿using UI;
using UnityEngine;

namespace MultiPlayer.Connection
{
    public class ServerConnectionWindow : MonoBehaviour
    {
        [SerializeField] private TextServerConnectionView _serverConnectionView;
        [SerializeField] private InspectorReferencedWindow _window;

        public IServerConnectionView ServerConnection => _serverConnectionView;
        public IWindow Window => _window;
    }
}