using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using GrpcWpfSample.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrpcWpfSample.Server
{
    public class ChatServer : Chat.ChatBase
    {
        public override Task Chat(IAsyncStreamReader<ChatLog> requestStream, IServerStreamWriter<ChatLog> responseStream, ServerCallContext context)
        {
            return base.Chat(requestStream, responseStream, context);
        }
    }
}
