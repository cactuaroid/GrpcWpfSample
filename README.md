# Overview

Chat server-client applications as a sample implementation using [gRPC for .NET (Grpc.AspNetCore/Grpc.Net.Client)](https://github.com/grpc/grpc-dotnet) and [gRPC for C# (Grpc.Core)](https://www.nuget.org/packages/Grpc.Core).

### Client
|Project|Type|Target|Used Package|
|:--|:--|:--|:--|
|GrpcChatSample.Client.ConsoleApp|Console app|.NET 6.0|Grpc.Core|
|GrpcChatSample.Client.Wpf|WPF app|.NET Framework 4.8|Grpc.Core|
|GrpcChatSample2.Client.ConsoleApp|Console app|.NET 6.0|Grpc.Net.Client|
|GrpcChatSample2.Client.Wpf|WPF app|.NET 6.0|Grpc.Net.Client|

### Server
|Project|Type|Target|Used Package|
|:--|:--|:--|:--|
|GrpcChatSample.Server.ConsoleApp|Console app|.NET 6.0|Grpc.Core|
|GrpcChatSample.Server.Wpf|WPF app|.NET Framework 4.8|Grpc.Core|
|GrpcChatSample2.Server.ConsoleApp|Console app|.NET 6.0|Grpc.AspNetCore|
|GrpcChatSample2.Server.Wpf|WPF app|.NET 6.0|Grpc.AspNetCore|

- These projects are not made from ASP.NET Core gRPC service project template, but adding gRPC service/client on console/WPF app project.
- .NET Framework apps are supported by Grpc.Core.
  - [Limited support](https://aka.ms/aspnet/grpc/netstandard) by Grpc.Net.Client but no sample in this repository.
- ASP.NET Core apps are supported by both, but Grpc.AspNetCore/Grpc.Net.Client is recommended.
- See https://grpc.io/blog/grpc-csharp-future/
- You cannot put proto file in WPF project. See https://github.com/grpc/grpc/issues/20402

Implements
- *Simple RPC*: Client registers a message to server.
- *Server-side streaming RPC*: Server streams messages to clients.

Read the official information
- https://grpc.io/docs/languages/csharp/
- https://grpc.io/docs/guides/
- https://docs.microsoft.com/ja-jp/aspnet/core/grpc/
- https://cloud.google.com/apis/design

### RPC Service Definition (chat.proto)

```proto
service Chat {
  rpc Write(ChatLog) returns (google.protobuf.Empty) {}
  rpc Subscribe(google.protobuf.Empty) returns (stream ChatLog) {}
}
```

### Architecture

![GrpcChatSample_archtecture](https://github.com/cactuaroid/GrpcWpfSample/blob/master/GrpcChatSample_archtecture.png)

DB is not implemented but on-memory `List` is used as a placeholder.

# How to Run

1. Start a server.
1. Start client(s). You can start multiple clients.
   - For the console app, write a name and hit enter key first.
1. On client, write a message and hit enter key.
1. Your message will be registered on server, and you can see it on the client.

# How to Enable SSL
### grpc-dotnet

See these classes. Enable `listenOptions.UseHttps()` of server and locate .pfx file. Set `var https = true` of client.

- [ChatGrpcServer.cs](https://github.com/cactuaroid/GrpcWpfSample/blob/master/grpc-dotnet/GrpcChatSample2.Server/Grpc/ChatGrpcServer.cs)
- [ChatServiceClient.cs](https://github.com/cactuaroid/GrpcWpfSample/blob/master/grpc-dotnet/GrpcChatSample2.Client/ChatServiceClient.cs)

### GrpcCore

See these classes. Locate required files and then change `var secure = true;` to enable SSL.

- [ChatGrpcServer.cs](https://github.com/cactuaroid/GrpcWpfSample/blob/master/GrpcCore/GrpcChatSample.Server/Grpc/ChatGrpcServer.cs)
- [ChatServiceClient.cs](https://github.com/cactuaroid/GrpcWpfSample/blob/master/GrpcCore/GrpcChatSample.Client/ChatServiceClient.cs)

|File Name|Description|
|:--|:--|
|localhost_server.crt   |Server's certificate|
|localhost_serverkey.pem|Private key of the server's certificate|
|localhost_client.crt   |Client's certificate (optional)|
|localhost_clientkey.pem|Private key of the client's certificate (optional)|

- All these files have to be PEM format without encryption.
- The server has to have the client's certificate or its CA certificate and vice versa. [Here](https://serverfault.com/questions/968343/why-do-i-need-a-certificate-to-establish-a-secure-grpc-connection-as-a-client) you can find an idea about this.
- The sample implementation includes client authentication as well. You can disable it then you don't need client's certificate and its private key. 

### Remarks
- You have to set "localhost" as Common Name (CN) of the certificate for your test on localhost.
- [XCA](https://hohnstaedt.de/xca/) is the one of the easy way to create the these files.
