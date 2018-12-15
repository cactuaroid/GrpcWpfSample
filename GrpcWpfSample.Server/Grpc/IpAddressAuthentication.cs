using Grpc.Core;
using Grpc.Core.Interceptors;
using GrpcWpfSample.Server.Infrastructure;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Threading.Tasks;

namespace GrpcWpfSample.Server.Rpc
{
    public class IpAddressAuthenticator : Interceptor
    {
        [Import]
        private Logger m_logger = null;

        public IpAddressAuthenticator() => MefManager.Container.ComposeParts(this);

        private readonly HashSet<string> m_authenticatedIps = new HashSet<string>()
        {
            "127.0.0.1",
        };

        private void VerifyPeer(ServerCallContext context)
        {
            var ip = context.Peer.Split(':')[1].Trim();

            context.Status = m_authenticatedIps.Contains(ip) ? 
                new Status(StatusCode.OK, $"Authenticated peer: {context.Peer}") :
                new Status(StatusCode.Unauthenticated, $"Unauthenticated peer: {context.Peer}");

            m_logger.Info(context.Status);
        }

        public override Task<TResponse> ClientStreamingServerHandler<TRequest, TResponse>(IAsyncStreamReader<TRequest> requestStream, ServerCallContext context, ClientStreamingServerMethod<TRequest, TResponse> continuation)
        {
            VerifyPeer(context);
            return base.ClientStreamingServerHandler(requestStream, context, continuation);
        }

        public override Task DuplexStreamingServerHandler<TRequest, TResponse>(IAsyncStreamReader<TRequest> requestStream, IServerStreamWriter<TResponse> responseStream, ServerCallContext context, DuplexStreamingServerMethod<TRequest, TResponse> continuation)
        {
            VerifyPeer(context);
            return base.DuplexStreamingServerHandler(requestStream, responseStream, context, continuation);
        }

        public override Task ServerStreamingServerHandler<TRequest, TResponse>(TRequest request, IServerStreamWriter<TResponse> responseStream, ServerCallContext context, ServerStreamingServerMethod<TRequest, TResponse> continuation)
        {
            VerifyPeer(context);
            return base.ServerStreamingServerHandler(request, responseStream, context, continuation);
        }

        public override Task<TResponse> UnaryServerHandler<TRequest, TResponse>(TRequest request, ServerCallContext context, UnaryServerMethod<TRequest, TResponse> continuation)
        {
            VerifyPeer(context);
            return base.UnaryServerHandler(request, context, continuation);
        }
    }
}
