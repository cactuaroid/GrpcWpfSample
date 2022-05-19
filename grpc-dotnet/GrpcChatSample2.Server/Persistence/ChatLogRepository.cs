using GrpcChatSample.Common;
using GrpcChatSample2.Server.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Reactive.Linq;

namespace GrpcChatSample2.Server.Persistence
{
    [Export(typeof(IChatLogRepository))]
    public class ChatLogRepository : IChatLogRepository
    {
        private readonly List<ChatLog> m_storage = new List<ChatLog>(); // dummy on memory storage

        public void Add(ChatLog chatLog)
        {
            m_storage.Add(chatLog);
        }

        public IEnumerable<ChatLog> GetAll()
        {
            return m_storage.AsReadOnly();
        }
    }
}
