using GameModes.Game;
using Networking.Client.Connection;
using UI;
using UI.Windows;
using UnityEngine;

namespace GameModes.MultiPlayer.Connection
{
    public class ServerConnectionView : IServerConnectionView
    {
        private readonly IServerConnectionView _connectionWindow;
        private readonly IWindow _window;

        public ServerConnectionView()
        {
            ServerConnectionViewConfiguration serverConnectionViewConfiguration =
                Resources.Load<ServerConnectionViewConfiguration>(GameResourceConfigurations
                    .ServerConnectionViewConfiguration);

            ServerConnectionWindow connectionWindow =
                Object.Instantiate(serverConnectionViewConfiguration.ConnectionWindow);

            _connectionWindow = connectionWindow.ServerConnection;
            _window = connectionWindow.Window;
            _window.Open();
        }

        public void DisplayConnecting()
        {
            _connectionWindow.DisplayConnecting();
        }

        public void DisplayError(string errorMessage)
        {
            _connectionWindow.DisplayError(errorMessage);
        }

        public void DisplayConnected()
        {
            _connectionWindow.DisplayConnected();
            _window.Close();
        }
    }
}