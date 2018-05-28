using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
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

namespace ClientApp1
{
    public partial class MainWindow : Window
    {
        private static int defaultPort = 3535;
        IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), defaultPort);
        Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Send_Clik(object sender, RoutedEventArgs e)
        {
            try
            {
                list.Items.Add("Oтправка сообщения...");
                socket.Connect(endPoint);
                ClientData clientData= new ClientData();
                var jsonConvert = JsonConvert.DeserializeObject<ClientData>(clientData.Sender);
                socket.Send(Encoding.Default.GetBytes(jsonConvert.Sender));

                StringBuilder stringBuilder = new StringBuilder();
                Socket incomeConnection = socket.Accept();
                byte[] data = new byte[1024];


                do
                {
                    stringBuilder.Append(Encoding.Default.GetString(data));
                }
                while(incomeConnection.Available > 0);

                List<ClientData> datas = new List<ClientData>();
                datas.Add(new ClientData {Sender=clientData.Sender,Text=text.Text,Time=DateTime.Now});

                //foreach (var item in datas)
                //{
                //    socket.Send()
                //}

            }
            catch (SocketException ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                socket.Close();
            }
        }
    }
}
