using Google.Protobuf.WellKnownTypes;
using GrpcChatSample.Common;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Windows.Data;

namespace GrpcChatSample2.Client.Wpf
{
    public class ChatClientWindowViewModel : BindableBase, IDisposable
    {
        private readonly ChatServiceClient m_chatService = new ChatServiceClient();

        public ObservableCollection<string> ChatHistory { get; } = new ObservableCollection<string>();
        private readonly object m_chatHistoryLockObject = new object();

        public string Name
        {
            get { return m_name; }
            set { SetProperty(ref m_name, value); }
        }
        private string m_name = "anonymous";

        private bool disposedValue;

        public DelegateCommand<string> WriteCommand { get; }

        public ChatClientWindowViewModel()
        {
            BindingOperations.EnableCollectionSynchronization(ChatHistory, m_chatHistoryLockObject);

            WriteCommand = new DelegateCommand<string>(WriteCommandExecute);

            StartReadingChatServer();
        }

        private void StartReadingChatServer()
        {
            var cts = new CancellationTokenSource();
            _ = m_chatService.ChatLogs()
                .ForEachAsync((x) => ChatHistory.Add($"{x.At.ToDateTime().ToString("HH:mm:ss")} {x.Name}: {x.Content}"), cts.Token);

            App.Current.Exit += (_, __) => cts.Cancel();
        }

        private async void WriteCommandExecute(string content)
        {
            await m_chatService.Write(new ChatLog
            {
                Name = m_name,
                Content = content,
                At = Timestamp.FromDateTime(DateTime.Now.ToUniversalTime()),
            });
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    m_chatService.Dispose();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
