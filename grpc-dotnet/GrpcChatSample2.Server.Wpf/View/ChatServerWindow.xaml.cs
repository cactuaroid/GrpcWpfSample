using GrpcChatSample2.Server.Wpf.ViewModel;
using System.Windows;

namespace GrpcChatSample2.Server.Wpf.View
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class ChatServerWindow : Window
    {
        public ChatServerWindow()
        {
            InitializeComponent();

            var viewModel = new ChatServerWindowViewModel();
            viewModel.SubscribeLogger();
            DataContext = viewModel;
        }
    }
}
