using Google.Protobuf.WellKnownTypes;
using GrpcWpfSample.Common;
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

namespace GrpcWpfSample.Client
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
