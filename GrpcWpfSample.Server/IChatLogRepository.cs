using GrpcWpfSample.Common;
using System.Collections.Generic;

namespace GrpcWpfSample.Server
{
    public interface IChatLogRepository
    {
        void Add(ChatLog chatLog);
        IAsyncEnumerable<ChatLog> GetAllAsync();
    }
}
