using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;

namespace Chat.models
{
    public class ConnecntModel
    {
        private Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        private CancellationTokenSource token = new CancellationTokenSource();

        public ConnecntModel(string ipAddress, string Name) {

            socket.Connect(ipAddress, 8888);
            SendMessage($"/username {Name}");
            
        }
        public async Task<string> ReciveMessage()
        {

            while (!token.IsCancellationRequested)
            {
                byte[] bytes = new byte[1024];
                await socket.ReceiveAsync(bytes, SocketFlags.None);
                string message = Encoding.UTF8.GetString(bytes);
                return  message;
            }
            return null;
        }

        public async Task SendMessage(string message)
        {
            if (message == "/exit")
            {
                token.Cancel();
                token.Dispose();
            }
            byte[] bytes = Encoding.UTF8.GetBytes(message);
            await socket.SendAsync(bytes, SocketFlags.None);
        }
    }
}
