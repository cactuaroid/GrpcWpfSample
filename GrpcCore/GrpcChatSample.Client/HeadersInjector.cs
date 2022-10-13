using Grpc.Core;
using Grpc.Core.Interceptors;
using System;
using System.Collections.Generic;
using System.Text;

namespace GrpcChatSample.Client
{
    /// <summary>
    /// By default, context.Options.Headers is null. This interceptor is to set empty Metadata there.
    ///   https://stackoverflow.com/questions/68442239/c-sharp-grpc-client-interceptor-set-authorization-header
    /// Call this interceptor before setup request header.
    /// </summary>
    public class HeadersInjector : Interceptor
    {
        private void SetHeaders<TRequest, TResponse>(ref ClientInterceptorContext<TRequest, TResponse> context) where TRequest : class where TResponse : class
        {
            if (context.Options.Headers == null)
            {
                var newOptions = context.Options.WithHeaders(new Metadata());
                context = new ClientInterceptorContext<TRequest, TResponse>(context.Method, context.Host, newOptions);
            }
        }

        public override AsyncClientStreamingCall<TRequest, TResponse> AsyncClientStreamingCall<TRequest, TResponse>(ClientInterceptorContext<TRequest, TResponse> context, AsyncClientStreamingCallContinuation<TRequest, TResponse> continuation)
        {
            SetHeaders(ref context);
            return base.AsyncClientStreamingCall(context, continuation);
        }

        public override AsyncDuplexStreamingCall<TRequest, TResponse> AsyncDuplexStreamingCall<TRequest, TResponse>(ClientInterceptorContext<TRequest, TResponse> context, AsyncDuplexStreamingCallContinuation<TRequest, TResponse> continuation)
        {
            SetHeaders(ref context);
            return base.AsyncDuplexStreamingCall(context, continuation);
        }

        public override AsyncServerStreamingCall<TResponse> AsyncServerStreamingCall<TRequest, TResponse>(TRequest request, ClientInterceptorContext<TRequest, TResponse> context, AsyncServerStreamingCallContinuation<TRequest, TResponse> continuation)
        {
            SetHeaders(ref context);
            return base.AsyncServerStreamingCall(request, context, continuation);
        }

        public override AsyncUnaryCall<TResponse> AsyncUnaryCall<TRequest, TResponse>(TRequest request, ClientInterceptorContext<TRequest, TResponse> context, AsyncUnaryCallContinuation<TRequest, TResponse> continuation)
        {
            SetHeaders(ref context);
            return base.AsyncUnaryCall(request, context, continuation);
        }

        public override TResponse BlockingUnaryCall<TRequest, TResponse>(TRequest request, ClientInterceptorContext<TRequest, TResponse> context, BlockingUnaryCallContinuation<TRequest, TResponse> continuation)
        {
            SetHeaders(ref context);
            return base.BlockingUnaryCall(request, context, continuation);
        }
    }
}
