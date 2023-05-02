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

namespace Msg
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

        private void soz_Click(object sender, RoutedEventArgs e)
        {
            if((!String.IsNullOrWhiteSpace(nik.Text.ToString())))
            {
                Server server = new Server();
                server.Name = nik.Text;
                server.Show();
                Close();
            }
            else
            {
                MessageBox.Show("Введите имя пользователя и ip адресс сети к которой ходите подключиться!");
            }
        }

        private void pod_Click(object sender, RoutedEventArgs e)
        {
            if((!String.IsNullOrWhiteSpace(nik.Text.ToString())) && (!String.IsNullOrWhiteSpace(ip.Text.ToString())))
            {
                Client client = new Client(ip.Text);
                client.Name = nik.Text;
                client.Show();
                Close();
            }
            else
            {
                MessageBox.Show("Введите имя пользователя и ip адресс сети к которой ходите подключиться!");
            }
            
        }
    }
}
