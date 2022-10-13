using Grpc.Core;
using Grpc.Core.Interceptors;
using GrpcChatSample2.Server.Infrastructure;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GrpcChatSample2.Server.Rpc
{
    /// <summary>
    /// CAUTION: This is not secure because IP address can be impersonated. Use SSL for secure client authentication.
    /// </summary>
    public class IpAddressAuthenticator : Interceptor
    {
        [Import]
        private Logger m_logger = null;

        public IpAddressAuthenticator() => MefManager.Container.ComposeParts(this);

        private readonly HashSet<string> m_authenticatedIps = new HashSet<string>()
        {
            "127.0.0.1",
            "::1"
        };

        private void VerifyPeer(ServerCallContext context)
        {
            context.Status = TryTakeIpAddress(context.Peer, out var ip) && m_authenticatedIps.Contains(ip) ?
                new Status(StatusCode.OK, $"Authenticated peer: {context.Peer}") :
                new Status(StatusCode.Unauthenticated, $"Unauthenticated peer: {context.Peer}");

            // reject unauthenticated peer
            if (context.Status.StatusCode == StatusCode.Unauthenticated)
            {
                m_logger.Info(context.Status);
                throw new RpcException(context.Status);
            }
        }

        private bool TryTakeIpAddress(string peer, out string ipAddress)
        {
            // ex.
            // "ipv4:127.0.0.1:12345"
            // "ipv6:[::1]:12345"

            var ipv4Match = Regex.Match(peer, @"^ipv4:(.+):");
            if (ipv4Match.Success)
            {
                ipAddress = ipv4Match.Groups[1].Value;
                return true;
            }

            var ipv6Match = Regex.Match(peer, @"^ipv6:\[(.+)\]");
            if (ipv6Match.Success)
            {
                ipAddress = ipv6Match.Groups[1].Value;
                return true;
            }

            ipAddress = "";
            return false;
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
