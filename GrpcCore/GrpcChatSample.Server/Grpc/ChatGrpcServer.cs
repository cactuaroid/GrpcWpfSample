using Grpc.Core;
using Grpc.Core.Interceptors;
using GrpcChatSample.Common;
using GrpcChatSample.Server.Infrastructure;
using System.ComponentModel.Composition;
using System.IO;

namespace GrpcChatSample.Server.Rpc
{
    [Export(typeof(IService))]
    public class ChatGrpcServer : IService
    {
        [Import]
        private Logger m_logger = null;

        [Import]
        private ChatGrpcService m_service = null;

        private readonly Grpc.Core.Server m_server;

        public ChatGrpcServer()
        {
            // Locate required files and set true to enable SSL
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
                        Chat.BindService(m_service)
                            .Intercept(new IpAddressAuthenticator())
                    },
                    Ports =
                    {
                        new ServerPort("localhost", 50052, credentials)
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
                        Chat.BindService(m_service)
                            .Intercept(new IpAddressAuthenticator())
                    },
                    Ports =
                    {
                        new ServerPort("localhost", 50052, ServerCredentials.Insecure)
                    }
                };
            }
        }

        public void Start()
        {
            m_server.Start();

            m_logger.Info("Started.");
        }
    }
}
