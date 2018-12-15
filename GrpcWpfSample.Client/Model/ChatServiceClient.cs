using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using GrpcWpfSample.Common;
using System;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace GrpcWpfSample.Client.Model
{
    class ChatServiceClient
    {
        private readonly Chat.ChatClient m_client =
            new Chat.ChatClient(
                new Channel("127.0.0.1:50052", ChannelCredentials.Insecure));

        public async Task Write(ChatLog chatLog)
        {
            await m_client.WriteAsync(chatLog);
        }

        public IObservable<ChatLog> ChatLogs()
        {
            var call = m_client.Subscribe(new Empty());

            return call.ResponseStream
                .ToAsyncEnumerable()
                .ToObservable()
                .Finally(() => call.Dispose());
        }
    }

}
