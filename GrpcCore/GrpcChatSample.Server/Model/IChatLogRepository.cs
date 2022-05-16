using GrpcChatSample.Common;
using System.Collections.Generic;

namespace GrpcChatSample.Server.Model
{
    public interface IChatLogRepository
    {
        void Add(ChatLog chatLog);
        IEnumerable<ChatLog> GetAll();
    }
}
