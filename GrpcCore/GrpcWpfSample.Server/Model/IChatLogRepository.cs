using GrpcWpfSample.Common;
using System.Collections.Generic;

namespace GrpcWpfSample.Server.Model
{
    public interface IChatLogRepository
    {
        void Add(ChatLog chatLog);
        IEnumerable<ChatLog> GetAll();
    }
}
