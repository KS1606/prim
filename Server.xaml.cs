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

namespace Msg
{
    public partial class Server : Window
    {
        private Socket socket;
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
                pol.Items.Add(client.RemoteEndPoint);
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

                sms.Items.Add($"[Cообщение от {client.RemoteEndPoint}]: {message}");

                foreach (var item in clints)
                {
                    SendMessage(item, message);
                }
            }
        }

        private async Task SendMessage(Socket client, string message)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(message);
            await client.SendAsync(bytes, SocketFlags.None);
        }

        private async Task send()
        {
            byte[] data = Encoding.UTF8.GetBytes(DateTime.Now.ToLongTimeString());
            string message = Encoding.UTF8.GetString(data);

            sms.Items.Add($"[Cообщение от {socket.RemoteEndPoint}]: {message}");

            foreach (var item in clints)
            {
                SendMessage(item, message);
            }
        }

        private void ot_Click(object sender, RoutedEventArgs e)
        {
            send();
        }
    }
}
