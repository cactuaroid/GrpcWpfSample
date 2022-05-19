using Google.Protobuf.WellKnownTypes;
using Grpc.Net.Client;
using GrpcChatSample.Common;

namespace GrpcChatSample2.Client
{
    public class ChatServiceClient
    {
        private readonly Chat.ChatClient m_client;

        public ChatServiceClient()
        {
            // See https://docs.microsoft.com/en-us/aspnet/core/grpc/client

            // To enable https, the server must be configured to use https.
            var https = true;

            if (https)
            {
                // See https://docs.microsoft.com/en-us/aspnet/core/grpc/authn-and-authz#client-certificate-authentication
                // for client certificate authentication

                var httpHandler = new HttpClientHandler();

                // Here you can disable validation for server certificate for your easy local test
                // See https://docs.microsoft.com/ja-jp/aspnet/core/grpc/troubleshoot#call-a-grpc-service-with-an-untrustedinvalid-certificate
                httpHandler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;

                m_client = new Chat.ChatClient(
                    GrpcChannel.ForAddress("https://localhost:50052", new GrpcChannelOptions { HttpHandler = httpHandler }));
            }
            else
            {
                // create insecure channel
                m_client = new Chat.ChatClient(
                    GrpcChannel.ForAddress("http://localhost:50052"));
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
