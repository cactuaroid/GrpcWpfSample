using System.Windows;
using System.Windows.Input;

namespace GrpcWpfSample.Client.View
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class ChatClientWindow : Window
    {
        public ChatClientWindow()
        {
            InitializeComponent();
            DataContext = new ChatClientWindowViewModel();
        }

        private void BodyTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                (DataContext as ChatClientWindowViewModel).WriteCommand.Execute(Body.Text);
                Body.Text = "";
            }
        }

        private void Body_Loaded(object sender, RoutedEventArgs e)
        {
            Body.Focus();
        }
    }
}
