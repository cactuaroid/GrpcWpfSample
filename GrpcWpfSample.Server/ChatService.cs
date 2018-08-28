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
        private IChatLogRepository m_repository = new ChatLogRepository(); // DI is better

        public override async Task Read(Empty request, IServerStreamWriter<ChatLog> responseStream, ServerCallContext context)
        {
            var oldItems = m_repository.GetAll();
            foreach (var item in oldItems)
            {
                await responseStream.WriteAsync(item);
            }

            while (true)
            {
                var newItems = m_repository.GetAll();
                var diffItems = newItems.Except(oldItems).ToArray();
                oldItems = newItems.ToArray();

                foreach (var item in diffItems)
                {
                    await responseStream.WriteAsync(item);
                }

                await Task.Delay(1000);
            }
        }

        public override Task<Empty> Write(ChatLog request, ServerCallContext context)
        {
            m_repository.Add(request);

            return Task.FromResult(new Empty());
        }
    }
}
