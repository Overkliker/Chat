using Chat.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Chat
{
    /// <summary>
    /// Логика взаимодействия для Server.xaml
    /// </summary>
    public partial class Server : Window
    {
        public ServerModel servers;
        CancellationTokenSource token = new CancellationTokenSource();

        public Server()
        {
            InitializeComponent();
            servers = new ServerModel();
            GetMess();
        }


        private void SendMessBtn_Click(object sender, RoutedEventArgs e)
        {
            string mess = SendBox.Text;
            MessagesLbx.Items.Add(servers.sendMess(mess));
            SendBox.Text = "";
        }

        private async Task GetMess()
        {
            while (!token.IsCancellationRequested)
            {
                await servers.ReciveMessage(servers.server);
            }

        }

        private void Window_Closed(object sender, EventArgs e)
        {
            servers.SendAllUser("/exit", "/exit");
        }

        private void ClosedBtn_Click(object sender, RoutedEventArgs e)
        {
            servers.SendAllUser("/exit", "/exit");
        }
    }

   
}
