using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using GrpcWpfSample.Common;
using GrpcWpfSample.Server.Infrastructure;
using GrpcWpfSample.Server.Model;
using System.ComponentModel.Composition;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace GrpcWpfSample.Server.Rpc
{
    [Export(typeof(IService))]
    public class ChatServiceGrpcServer : Chat.ChatBase, IService
    {
        [Import]
        private Logger m_logger = null;

        [Import]
        private ChatService m_chatService = null;
        private readonly Empty m_empty = new Empty();

        private const int Port = 50052;
        private readonly Grpc.Core.Server m_server;

        public ChatServiceGrpcServer()
        {
            m_server = new Grpc.Core.Server
            {
                Services = { Chat.BindService(this) },
                Ports = { new ServerPort("localhost", Port, ServerCredentials.Insecure) }
            };
        }

        public void Start()
        {
            m_server.Start();

            m_logger.Info("Started.");
        }

        public override async Task Subscribe(Empty request, IServerStreamWriter<ChatLog> responseStream, ServerCallContext context)
        {
            m_logger.Info($"{context.Host} subscribes.");

            context.CancellationToken.Register(() => m_logger.Info($"{context.Host} unsubscribed."));

            await m_chatService.GetChatLogsAsObservable()
                .ToAsyncEnumerable()
                .ForEachAsync(async (x) => await responseStream.WriteAsync(x)); // runs sequentially

            // never completes
        }

        public override Task<Empty> Write(ChatLog request, ServerCallContext context)
        {
            m_logger.Info($"{context.Host} {request}");

            m_chatService.Add(request);

            return Task.FromResult(m_empty);
        }
    }
}
