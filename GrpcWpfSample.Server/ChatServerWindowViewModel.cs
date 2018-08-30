using GrpcWpfSample.Server.Persistence;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace GrpcWpfSample.Server
{
    public class ChatServerWindowViewModel
    {
        private ChatServer m_chatServer = new ChatServer();

        public ObservableCollection<string> ChatHistory { get; private set; } = new ObservableCollection<string>();
        private object m_chatHistoryLockObject = new object();

        public ChatServerWindowViewModel()
        {
            BindingOperations.EnableCollectionSynchronization(ChatHistory, m_chatHistoryLockObject);
        }

        public void StartServer()
        {
            m_chatServer.Start();

            Task.Run(async () =>
                await m_chatServer.GetAllAsync()
                    .ForEachAsync((x) => ChatHistory.Add(x)));
        }
    }
}
