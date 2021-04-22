using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Grpc.Core.Interceptors;
using GrpcWpfSample.Common;
using GrpcWpfSample.Server.Infrastructure;
using GrpcWpfSample.Server.Model;
using System.ComponentModel.Composition;
using System.IO;
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
            var secure = false;

            if (secure)
            {
                // secure
                var clientCACert = File.ReadAllText(@"C:\localhost_client.crt");
                var serverCert = File.ReadAllText(@"C:\localhost_server.crt");
                var serverKey = File.ReadAllText(@"C:\localhost_serverkey.pem");
                var keyPair = new KeyCertificatePair(serverCert, serverKey);
                var credentials = new SslServerCredentials(new[] { keyPair }, clientCACert, SslClientCertificateRequestType.RequestAndRequireAndVerify);

                // Client authentication is an option. You can remove it as follows if you only need SSL.
                //var credentials = new SslServerCredentials(new[] { keyPair });

                m_server = new Grpc.Core.Server
                {
                    Services =
                    {
                        Chat.BindService(this)
                            .Intercept(new IpAddressAuthenticator())
                    },
                    Ports =
                    {
                        new ServerPort("localhost", Port, credentials)
                    }
                };
            }
            else
            {
                // insecure
                m_server = new Grpc.Core.Server
                {
                    Services =
                    {
                        Chat.BindService(this)
                            .Intercept(new IpAddressAuthenticator())
                    },
                    Ports =
                    {
                        new ServerPort("localhost", Port, ServerCredentials.Insecure)
                    }
                };
            }
        }

        public void Start()
        {
            m_server.Start();

            m_logger.Info("Started.");
        }

        public override async Task Subscribe(Empty request, IServerStreamWriter<ChatLog> responseStream, ServerCallContext context)
        {
            m_logger.Info($"{context.Host} subscribes.");

            context.CancellationToken.Register(() => m_logger.Info($"{context.Host} cancels subscription."));

            // Completing the method means disconnecting the stream by server side.
            // If subscribing IObservable, you have to block this method after the subscription.
            // I prefer converting IObservable to IAsyncEnumerable to consume the sequense here
            // because gRPC interface is in IAsyncEnumerable world.
            // Note that the chat service model itself is in IObservable world
            // because chat is naturally recognized as an event sequence.

            try
            {
                await m_chatService.GetChatLogsAsObservable()
                    .ToAsyncEnumerable()
                    .ForEachAwaitAsync(async (x) => await responseStream.WriteAsync(x), context.CancellationToken)
                    .ConfigureAwait(false);
            }
            catch (TaskCanceledException)
            {
                m_logger.Info($"{context.Host} unsubscribed.");
            }
        }

        public override Task<Empty> Write(ChatLog request, ServerCallContext context)
        {
            m_logger.Info($"{context.Host} {request}");

            m_chatService.Add(request);

            return Task.FromResult(m_empty);
        }
    }
}
