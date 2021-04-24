using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using GrpcWpfSample.Common;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GrpcWpfSample.Client
{
    public class ChatServiceClient
    {
        private readonly Chat.ChatClient m_client;

        public ChatServiceClient()
        {
            var secure = false;

            if (secure)
            {
                // create secure channel
                var serverCACert = File.ReadAllText(@"C:\localhost_server.crt");
                var clientCert = File.ReadAllText(@"C:\localhost_client.crt");
                var clientKey = File.ReadAllText(@"C:\localhost_clientkey.pem");
                var keyPair = new KeyCertificatePair(clientCert, clientKey);
                var credentials = new SslCredentials(serverCACert, keyPair);

                m_client = new Chat.ChatClient(
                    new Channel("localhost", 50052, credentials));
            }
            else
            {
                // create insecure channel
                m_client = new Chat.ChatClient(
                    new Channel("localhost", 50052, ChannelCredentials.Insecure));
            }
        }

        public async Task Write(ChatLog chatLog)
        {
            await m_client.WriteAsync(chatLog);
        }

        public IAsyncEnumerable<ChatLog> ChatLogs()
        {
            var call = m_client.Subscribe(new Empty());

            // I do not want to expose gRPC such as IAsyncStreamReader or AsyncServerStreamingCall.
            // I also do not want to bother user of this class with asking to dispose the call object.

            return call.ResponseStream
                .ToAsyncEnumerable()
                .Finally(() => call.Dispose());
        }
    }
}
