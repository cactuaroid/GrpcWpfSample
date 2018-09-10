using GrpcWpfSample.Server.Model;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Reactive.Linq;
using System.Windows.Data;

namespace GrpcWpfSample.Server.ViewModel
{
    public class ChatServerWindowViewModel
    {
        [Import]
        private ChatService m_chatService = null;

        public ObservableCollection<string> ChatHistory { get; private set; } = new ObservableCollection<string>();

        public ChatServerWindowViewModel()
        {
            MefManager.Container.ComposeParts(this);
            BindingOperations.EnableCollectionSynchronization(ChatHistory, new object());
        }

        public void SubscribeChatService()
        {
            m_chatService.GetChatLogsAsObservable()
                .Select((x) => x.ToString())
                .Subscribe((x) => ChatHistory.Add(x));
        }
    }
}
