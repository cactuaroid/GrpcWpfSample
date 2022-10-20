using Google.Protobuf.WellKnownTypes;
using Grpc.Core.Interceptors;
using Grpc.Net.Client;
using GrpcChatSample.Common;
using System.Threading.Channels;

namespace GrpcChatSample2.Client
{
    public class ChatServiceClient : IDisposable
    {
        private readonly Chat.ChatClient m_client;
        private readonly GrpcChannel m_channel; // If you create multiple client instances, the GrpcChannel should be shared.
        private bool disposedValue;

        public ChatServiceClient()
        {
            // See https://docs.microsoft.com/en-us/aspnet/core/grpc/client

            // To enable https, the server must be configured to use https.
            var https = false;

            if (https)
            {
                // See https://docs.microsoft.com/en-us/aspnet/core/grpc/authn-and-authz#client-certificate-authentication
                // for client certificate authentication

                var httpHandler = new HttpClientHandler();

                // Here you can disable validation for server certificate for your easy local test
                // See https://docs.microsoft.com/en-us/aspnet/core/grpc/troubleshoot#call-a-grpc-service-with-an-untrustedinvalid-certificate
                //httpHandler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;

                m_channel = GrpcChannel.ForAddress("https://localhost:50052", new GrpcChannelOptions { HttpHandler = httpHandler });
                m_client = new Chat.ChatClient(
                    m_channel
                    .Intercept(new ClientIdInjector()) // 2nd
                    .Intercept(new HeadersInjector())); // 1st
            }
            else
            {
                // create insecure channel
                m_channel = GrpcChannel.ForAddress("http://localhost:50052");
                m_client = new Chat.ChatClient(
                    m_channel
                    .Intercept(new ClientIdInjector()) // 2nd
                    .Intercept(new HeadersInjector())); // 1st
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

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    m_channel.Dispose(); // disposes all of active calls.
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
