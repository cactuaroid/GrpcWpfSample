using GrpcWpfSample.Server.Model;
using System;
using System.Collections.ObjectModel;
using System.Reactive.Linq;
using System.Windows.Data;

namespace GrpcWpfSample.Server.ViewModel
{
    public class ChatServerWindowViewModel
    {
        private readonly ChatServer m_chatServer = new ChatServer();

        public ObservableCollection<string> ChatHistory { get; private set; } = new ObservableCollection<string>();
        private readonly object m_chatHistoryLockObject = new object();

        public ChatServerWindowViewModel()
        {
            BindingOperations.EnableCollectionSynchronization(ChatHistory, m_chatHistoryLockObject);
        }

        public void StartServer()
        {
            m_chatServer.Start();
            m_chatServer.GetAllAsync()
                .Subscribe((x) => ChatHistory.Add(x));
        }
    }
}
