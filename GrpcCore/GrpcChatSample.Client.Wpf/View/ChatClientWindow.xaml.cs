﻿using System.Windows;
using System.Windows.Input;

namespace GrpcWpfSample.Client.Wpf.View
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

        private void BodyInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                (DataContext as ChatClientWindowViewModel).WriteCommand.Execute(BodyInput.Text);
                BodyInput.Text = "";
            }
        }

        private void BodyInput_Loaded(object sender, RoutedEventArgs e)
        {
            BodyInput.Focus();
        }
    }
}
