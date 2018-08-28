using GrpcWpfSample.Common;
using System.Collections.Generic;

namespace GrpcWpfSample.Server.Persistence
{
    public class ChatLogRepository : IChatLogRepository
    {
        private List<ChatLog> m_storage = new List<ChatLog>();

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
