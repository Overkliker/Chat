using Chat.models;
using System;
using System.Collections.Generic;
using System.Linq;
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
    /// Логика взаимодействия для Connect.xaml
    /// </summary>
    public partial class Connect : Window
    {

        private CancellationTokenSource token = new CancellationTokenSource();

        private ConnecntModel connecntModel;
        public Connect(string ip, string name)
        {
            InitializeComponent();
            connecntModel = new ConnecntModel(ip, name);
            GetMess();

            CommLbx.Items.Add("/username");
            CommLbx.Items.Add("/exit");
            CommLbx.Items.Add("/allusers");
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            connecntModel.SendMessage(MessageString.Text);

            MessageString.Text = "";
        }

        private async Task GetMess()
        {
            while (!token.IsCancellationRequested)
            {
                string message = await connecntModel.ReciveMessage();
                if (message != null)
                {
                    if (message.Replace("\0", string.Empty) == "$/exit$")
                    {
                        token.Cancel();
                        this.Close();
                    }

                    MessageLbx.Items.Add(message);
                }

                
            }

        }


    }
}
