using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Chat
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

        }

        private void ConnectBtn_Click(object sender, RoutedEventArgs e)
        {
            if (IP.Text != null && Login.Text != null)
            {
                Connect server = new Connect(IP.Text, Login.Text);
                server.Show();
            }
        }

        private void MakeServerBtn_Click(object sender, RoutedEventArgs e)
        {
            if (Login.Text != null)
            {
                Server server = new Server();
                server.Show();
            }

        }
    }
}
