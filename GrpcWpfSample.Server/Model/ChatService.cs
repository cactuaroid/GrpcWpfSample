using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using GrpcWpfSample.Common;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace GrpcWpfSample.Server.Model
{
    public class ChatService : Chat.ChatBase
    {
        private readonly IChatLogRepository m_repository;
        private readonly Empty m_empty = new Empty();
        public ChatService(IChatLogRepository repository) => m_repository = repository;

        public override async Task Subscribe(Empty request, IServerStreamWriter<ChatLog> responseStream, ServerCallContext context)
        {
            await m_repository.GetAllAsync()
                .ForEachTaskAsync(async (x) => await responseStream.WriteAsync(x));

            // never completes
        }

        public override Task<Empty> Write(ChatLog request, ServerCallContext context)
        {
            // If this Write() directly write to the 'responseStream' of Subscribe(), it raises exception because these are in different RPC calling.
            // It is same even if using event, because it is actually just invoking some methods. IObservable neither.
            // The only way to trigger writing to the stream which is awaited in different RPC calling is that
            // 'await' a signal (Task completion) on stream owner side and trigger the task completion on the other side.
            // This is why I implement ChatLogRepository with AsyncAutoResetEvent.

            m_repository.Add(request);

            return Task.FromResult(m_empty);
        }
    }
}
