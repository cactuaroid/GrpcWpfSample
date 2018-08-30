using GrpcWpfSample.Common;
using GrpcWpfSample.Server.Model;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;

namespace GrpcWpfSample.Server.Persistence
{
    public class ChatLogRepository : IChatLogRepository
    {
        private readonly List<ChatLog> m_storage = new List<ChatLog>();
        private readonly AsyncAutoResetEvent<ChatLog> m_signal = new AsyncAutoResetEvent<ChatLog>();

        public void Add(ChatLog chatLog)
        {
            m_storage.Add(chatLog);
            m_signal.Set(chatLog);
        }

        public IAsyncEnumerable<ChatLog> GetAllAsync()
        {
            var oldLogs = m_storage.ToAsyncEnumerable();
            var newLogs = AsyncEnumerable.CreateEnumerable(() =>
                AsyncEnumerable.CreateEnumerator(
                    async (token) =>
                    {
                        await m_signal.WaitAsync();
                        return true; // the sequence never completes
                    },
                    () => m_signal.Current,
                    () => { }));

            return oldLogs.Concat(newLogs);
        }
    }
}
