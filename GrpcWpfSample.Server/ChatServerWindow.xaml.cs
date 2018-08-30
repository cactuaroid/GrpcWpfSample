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
            DataContext = new ChatServerWindowViewModel();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            (DataContext as ChatServerWindowViewModel).StartServer();
        }
    }
}
