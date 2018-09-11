using GrpcWpfSample.Server.ViewModel;
using System.Windows;

namespace GrpcWpfSample.Server.View
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
