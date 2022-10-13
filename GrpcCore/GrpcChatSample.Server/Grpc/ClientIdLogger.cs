using Grpc.Core;
using Grpc.Core.Interceptors;
using GrpcChatSample.Server.Infrastructure;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Threading.Tasks;

namespace GrpcChatSample.Server
{
    /// <summary>
    /// This interceptor is a sample for reading request header.
    /// </summary>
    public class ClientIdLogger : Interceptor
    {
        [Import]
        private Logger m_logger = null;

        public ClientIdLogger() => MefManager.Container.ComposeParts(this);

        private void LogClientId(ServerCallContext context)
        {
            m_logger.Info($"client_id: {context.RequestHeaders.GetValue("client_id")}");
        }

        public override Task<TResponse> ClientStreamingServerHandler<TRequest, TResponse>(IAsyncStreamReader<TRequest> requestStream, ServerCallContext context, ClientStreamingServerMethod<TRequest, TResponse> continuation)
        {
            LogClientId(context);
            return base.ClientStreamingServerHandler(requestStream, context, continuation);
        }

        public override Task DuplexStreamingServerHandler<TRequest, TResponse>(IAsyncStreamReader<TRequest> requestStream, IServerStreamWriter<TResponse> responseStream, ServerCallContext context, DuplexStreamingServerMethod<TRequest, TResponse> continuation)
        {
            LogClientId(context);
            return base.DuplexStreamingServerHandler(requestStream, responseStream, context, continuation);
        }

        public override Task ServerStreamingServerHandler<TRequest, TResponse>(TRequest request, IServerStreamWriter<TResponse> responseStream, ServerCallContext context, ServerStreamingServerMethod<TRequest, TResponse> continuation)
        {
            LogClientId(context);
            return base.ServerStreamingServerHandler(request, responseStream, context, continuation);
        }

        public override Task<TResponse> UnaryServerHandler<TRequest, TResponse>(TRequest request, ServerCallContext context, UnaryServerMethod<TRequest, TResponse> continuation)
        {
            LogClientId(context);
            return base.UnaryServerHandler(request, context, continuation);
        }
    }
}
