using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using GrpcWpfSample.Common;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GrpcWpfSample.Client
{
    public class ChatServiceClient
    {
        private readonly Chat.ChatClient m_client =
            new Chat.ChatClient(
                new Channel("127.0.0.1:50052", ChannelCredentials.Insecure));

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
