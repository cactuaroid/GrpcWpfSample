using GrpcChatSample2.Server.Infrastructure;
using GrpcChatSample2.Server.Rpc;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel.Composition;
using System.Net;

namespace GrpcChatSample2.Server.Grpc
{
    [Export(typeof(IService))]
    internal class ChatGrpcServer : IService
    {
        [Import]
        private Logger m_logger = null;

        [Import]
        private ChatGrpcService m_service = null;

        private WebApplication m_app;

        public void Start()
        {
            // See
            // https://docs.microsoft.com/en-us/aspnet/core/grpc/aspnetcore
            // https://docs.microsoft.com/en-us/aspnet/core/fundamentals/servers/kestrel/endpoints

            var builder = WebApplication.CreateBuilder();

            // Add services to the container.
            builder.Services.AddGrpc((options) =>
            {
                // See https://docs.microsoft.com/en-us/aspnet/core/grpc/interceptors#server-interceptors
                options.Interceptors.Add<IpAddressAuthenticator>();
            });

            // See https://docs.microsoft.com/en-us/dotnet/core/extensions/dependency-injection#service-lifetimes
            builder.Services.AddSingleton(m_service);
            builder.Services.AddSingleton(new IpAddressAuthenticator());

            builder.WebHost.ConfigureKestrel(serverOptions =>
            {
                serverOptions.Listen(IPAddress.Any, 50052, (listenOptions) =>
                {
                    listenOptions.Protocols = HttpProtocols.Http2;

                    // HTTPS is recommended
                    //listenOptions.UseHttps(@"C:\localhost_server.pfx", "password");
                });
            });

            m_app = builder.Build();

            m_app.MapGrpcService<ChatGrpcService>();

            m_app.RunAsync();
            m_logger.Info("Started.");
        }
    }
}
