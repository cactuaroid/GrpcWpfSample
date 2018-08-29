using Google.Protobuf.WellKnownTypes;
using GrpcWpfSample.Common;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Data;

namespace GrpcWpfSample.Client
{
    public class ChatClientWindowViewModel : BindableBase
    {
        private ChatServiceClient m_chatService = new ChatServiceClient();

        public ObservableCollection<string> ChatHistory { get; private set; } = new ObservableCollection<string>();
        private object m_chatHistoryLockObject = new object();

        public string Name
        {
            get { return m_name; }
            set { SetProperty(ref m_name, value); }
        }
        private string m_name = "anonymous";

        public DelegateCommand<string> WriteCommand { get; private set; }

        public ChatClientWindowViewModel()
        {
            BindingOperations.EnableCollectionSynchronization(ChatHistory, m_chatHistoryLockObject);

            WriteCommand = new DelegateCommand<string>(WriteCommandExecute);

            StartReadingChatServer();
        }

        private void StartReadingChatServer()
        {
            Task.Run(async () =>
                await m_chatService.Subscribe((x) =>
                    ChatHistory.Add($"{x.At.ToDateTime().ToString("HH:mm:ss")} {x.Name}: {x.Content}")));
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
    }
}
