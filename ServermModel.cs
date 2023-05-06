using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Chat
{
    internal class ServermModel
    {
        public static string ValidMessage(Socket Name, List<SocketModel> clients, SocketModel server, string message, bool isServer = false)
        {
            return $"{(isServer == false ? clients.FirstOrDefault(element => element.socket == Name).Name : server.Name)} {DateTime.Now}: {message} \n".Replace("\0", "");
        }

    }
}
