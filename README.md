# Overview

Chat server-client applications as a sample implementation of gRPC on C#
- WPF app on .NET Framework 4.8
- Console app on .NET 5.0
- Their common libraries on .NET Standard 2.0

implements:
- *Simple RPC*: Client registers a message to server.
- *Server-side streaming RPC*: Server streams messages to clients.

Read the official [quick start](https://grpc.io/docs/quickstart/csharp.html) and [guides](https://grpc.io/docs/guides/).

### Architecture

![GrpcWpfSample_archtecture](https://github.com/cactuaroid/GrpcWpfSample/blob/master/GrpcWpfSample_archtecture.png)

Note that  DB is not implemented but on-memory `List` is used as a placeholder.

### RPC Service Definition (chat.proto)

```proto
service Chat {
  rpc Write(ChatLog) returns (google.protobuf.Empty) {}
  rpc Subscribe(google.protobuf.Empty) returns (stream ChatLog) {}
}
```

# How to Run

|Project|Type|
|:--|:--|
|GrpcWpfSample.Server.Wpf|Server on .NET Framework WPF app|
|GrpcWpfSample.Server.Core|Server on .NET Core Console app|
|GrpcWpfSample.Client.Wpf|Client on .NET Framework WPF app|
|GrpcWpfSample.Client.Core|Client on .NET Core Console app|

1. Start a server.
1. Start client(s). You can start multiple clients.
   - For GrpcWpfSample.Client.Core, write a name and hit enter key.
1. On client, write a message and hit enter key.
1. Your message will be registered on server, and you can see it on the client.

# How to Enable SSL

See these classes. Locate required files and then change `var secure = true;` to enable SSL.

- [ChatServiceGrpcServer.cs](https://github.com/cactuaroid/GrpcWpfSample/blob/master/GrpcWpfSample.Server/Grpc/ChatServiceGrpcServer.cs)
- [ChatServiceClient.cs](https://github.com/cactuaroid/GrpcWpfSample/blob/master/GrpcWpfSample.Client/ChatServiceClient.cs)

|File Name|Description|
|:--|:--|
|localhost_server.crt   |Server's certificate|
|localhost_serverkey.pem|Private key of the server's certificate|
|localhost_client.crt   |Client's certificate (optional)|
|localhost_clientkey.pem|Private key of the client's certificate (optional)|

- All these files have to be PEM format without enryption.
- The server has to have the client's certificate or its CA certificate and vice versa. [Here](https://serverfault.com/questions/968343/why-do-i-need-a-certificate-to-establish-a-secure-grpc-connection-as-a-client) you can find an idea about this.
- The sample implementation includes client authentication as well. You can disable it then you don't need client's certificate and its private key. 
- You have to set "localhost" as Common Name (CN) of the certificate for your test on localhost.
- [XCA](https://hohnstaedt.de/xca/) is the one of the easy way to create the these files.
