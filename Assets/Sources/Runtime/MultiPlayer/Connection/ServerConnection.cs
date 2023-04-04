using System.Net.Sockets;
using System.Threading.Tasks;

namespace MultiPlayer.Connection
{
    public class ServerConnection
    {
        private readonly IServerConnectionView _serverConnectionView;
        private TcpClient _tcpClient;

        public ServerConnection(IServerConnectionView serverConnectionView)
        {
            _serverConnectionView = serverConnectionView;
        }

        public TcpClient Client => _tcpClient;

        public async Task<bool> Connect()
        {
            TcpClient tcpClient = new TcpClient();
            _serverConnectionView.DisplayConnecting();

            try
            {
                await tcpClient.ConnectAsync("192.168.1.87", 55555);
            }
            catch (SocketException exception)
            {
                if (exception.ErrorCode.Equals(10061))
                {
                    _serverConnectionView.DisplayError("Server is unavailable");
                    return false;
                }

                _serverConnectionView.DisplayError(exception.ToString());
                return false;
            }

            _serverConnectionView.DisplayConnected();
            _tcpClient = tcpClient;

            return true;
        }
    }
}