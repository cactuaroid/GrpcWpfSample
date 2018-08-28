using Grpc.Core;
using GrpcWpfSample.Common;
using System.Windows;

namespace GrpcWpfSample.Server
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class ChatServerWindow : Window
    {
        public ChatServerWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Start();
        }

        private void Start()
        {
            const int Port = 50052;

            var server = new Grpc.Core.Server
            {
                Services = { Chat.BindService(new ChatService()) },
                Ports = { new ServerPort("localhost", Port, ServerCredentials.Insecure) }
            };
            server.Start();
        }
    }
}
