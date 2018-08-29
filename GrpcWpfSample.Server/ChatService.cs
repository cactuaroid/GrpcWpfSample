using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using GrpcWpfSample.Common;
using GrpcWpfSample.Server.Persistence;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace GrpcWpfSample.Server
{
    public class ChatService : Chat.ChatBase
    {
        private IChatLogRepository m_repository = new ChatLogRepository(); // DI is better for separating persistence layer

        public override async Task Subscribe(Empty request, IServerStreamWriter<ChatLog> responseStream, ServerCallContext context)
        {
            // WriteAsync() has to be 'Wait()' because calling WriteAsync() has to be one by one, not parallelly.
            await m_repository.GetAllAsync()
                .ForEachAsync((x) => responseStream.WriteAsync(x).Wait());

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

            return Task.FromResult(new Empty());
        }
    }
}
