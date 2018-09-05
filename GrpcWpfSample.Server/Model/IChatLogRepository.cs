using GrpcWpfSample.Common;
using System;

namespace GrpcWpfSample.Server.Model
{
    public interface IChatLogRepository
    {
        void Add(ChatLog chatLog);
        IObservable<ChatLog> GetAllAsync();
    }
}
