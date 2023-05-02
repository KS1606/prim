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

namespace Msg
{
    /// <summary>
    /// Логика взаимодействия для Client.xaml
    /// </summary>
    public partial class Client : Window
    {
        private Socket socket;
        public string Name;
        CancellationTokenSource currentOperationTioken;
        Task currentOperation;
        public Client(string ip)
        {
            InitializeComponent();
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Connect(Convert.ToString(ip), 8888);
            RecieveMessage();
        }

        private async Task RecieveMessage()
        {
            while (true)
            {
                byte[] bytes = new byte[1024];
                await socket.ReceiveAsync(bytes, SocketFlags.None);
                string messege = Encoding.UTF8.GetString(bytes);
                sms.Items.Add(messege);
            }
        }

        private async Task send(string msg)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(msg);
            await socket.SendAsync(bytes, SocketFlags.None);
        }

        private void ot_Click(object sender, RoutedEventArgs e)
        {
            if (vvod_sms.Text == "/disconnect")
            {
                string msg = vvod_sms.Text;
                send("0");
                disconnect();
            }
            else
            {
                string msg = " [ " + DateTime.Now.ToString("dd/MM/yyyy") + "  " + DateTime.Now.ToLongTimeString() + " ] " + Name + " :" + vvod_sms.Text;
                send(msg);
            }
            
        }

        private void v_Click(object sender, RoutedEventArgs e)
        {
            string msg = "/disconnect";
            send("0");
            disconnect();
        }

        private async void disconnect()
        {
            currentOperationTioken = new CancellationTokenSource();
            currentOperationTioken.Cancel();
            MainWindow window = new MainWindow();
            window.Show();
            Close();
        }
    }
}
