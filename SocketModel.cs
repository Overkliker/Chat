using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Chat
{
    public class SocketModel
    {
        public Socket socket { get; set; }
        public string Name { get; set; }
        public CancellationTokenSource token { get; set; }
        public int role { get; set; }
    }
}
