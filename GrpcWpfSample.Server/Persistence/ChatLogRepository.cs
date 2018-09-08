using GrpcWpfSample.Common;
using GrpcWpfSample.Server.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;

namespace GrpcWpfSample.Server.Persistence
{
    public class ChatLogRepository : IChatLogRepository
    {
        private readonly List<ChatLog> m_storage = new List<ChatLog>();
        private event Action<ChatLog> Added;

        public void Add(ChatLog chatLog)
        {
            m_storage.Add(chatLog);
            Added?.Invoke(chatLog);
        }

        public IObservable<ChatLog> GetAllAsync()
        {
            var oldLogs = m_storage.ToObservable();
            var newLogs = Observable.FromEvent<ChatLog>((x) => Added += x, (x) => Added -= x);

            return oldLogs.Concat(newLogs);
        }
    }
}
