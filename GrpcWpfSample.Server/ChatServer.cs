using Grpc.Core;
using GrpcWpfSample.Common;
using GrpcWpfSample.Server.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrpcWpfSample.Server
{
    public class ChatServer
    {
        private const int Port = 50052;
        private IChatLogRepository m_repository = new ChatLogRepository(); // Using IoC container is better for separating persistence layer

        public void Start()
        {
            var server = new Grpc.Core.Server
            {
                Services = { Chat.BindService(new ChatService(m_repository)) },
                Ports = { new ServerPort("localhost", Port, ServerCredentials.Insecure) }
            };
            server.Start();
        }

        public IAsyncEnumerable<string> GetAllAsync()
        {
            return m_repository.GetAllAsync().Select((x) => x.ToString());
        }
    }
}
