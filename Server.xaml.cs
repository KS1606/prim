using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Collections;

namespace Msg
{
    public partial class Server : Window
    {
        private Socket socket;
        public string Name;
        private List<Socket> clints = new List<Socket>();
        public Server()
        {
            InitializeComponent();
            IPEndPoint ipPoint = new IPEndPoint(IPAddress.Any, 8888);
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Bind(ipPoint);
            socket.Listen(1000);

            ListenToClients();
        }

        private async Task ListenToClients()
        {
            pol.Items.Clear();
            while (true)
            {
                var client = await socket.AcceptAsync();
                clints.Add(client);
                pol.Items.Add( "Новый пользователь: " + client.RemoteEndPoint + " " + " [ " + DateTime.Now.ToString("dd/MM/yyyy") + "  " + DateTime.Now.ToLongTimeString() + " ] ");
                RecieveMessage(client);
            }

        }

        private async Task RecieveMessage(Socket client)
        {
            while (true)
            {
                byte[] bytes = new byte[1024];
                await client.ReceiveAsync(bytes, SocketFlags.None);
                string message = Encoding.UTF8.GetString(bytes);
/*                string msg = Convert.ToString("0");
                MessageBox.Show(Convert.ToString(bytes[0]));
                MessageBox.Show(message);*/
                if (bytes[0] == 48 )
                {
                    pol.Items.Add("Выход пользователя: " + client.RemoteEndPoint + " " + "Время: " + " [ " + DateTime.Now.ToString("dd/MM/yyyy") + "  " + DateTime.Now.ToLongTimeString() + " ] ");
                }
                else
                {
                    sms.Items.Add(message);

                    foreach (var item in clints)
                    {
                        SendMessage(item, message);
                    }
                }
            }
        }

        private async Task SendMessage(Socket client, string message)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(message);
            await client.SendAsync(bytes, SocketFlags.None);
        }

        private async Task send(string msg)
        {
            byte[] data = Encoding.UTF8.GetBytes(msg);
            string message = Encoding.UTF8.GetString(data);

            sms.Items.Add(message);

            foreach (var item in clints)
            {
                SendMessage(item, message);
            }
        }

        private void ot_Click(object sender, RoutedEventArgs e)
        {
            string msg = " [ " + DateTime.Now.ToString("dd/MM/yyyy") + "  " + DateTime.Now.ToLongTimeString() + " ] " + Name + " :" + vvod_sms.Text;
            send(msg);
        }

        private void v_Click(object sender, RoutedEventArgs e)
        {
            MainWindow window = new MainWindow();
            window.Show();
            Close();
        }
    }
}
