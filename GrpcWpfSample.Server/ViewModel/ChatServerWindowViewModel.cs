using GrpcWpfSample.Server.Model;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Data;

namespace GrpcWpfSample.Server.ViewModel
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
