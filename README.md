# Overview

Chat server-client applications as a sample implementation of gRPC on C#
- WPF app on .NET Framework 4.8
- Console app on .NET Core 2.0

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

# How to run

|EXE|TYPE|
|:--|:--|
|GrpcWpfSample.Server.Wpf.exe|Server on .NET Framework WPF app|
|GrpcWpfSample.Server.Core.exe|Server on .NET Core Console app|
|GrpcWpfSample.Client.Wpf.exe|Client on .NET Framework WPF app|
|GrpcWpfSample.Client.Core.exe|Client on .NET Core Console app|

1. Start a server.
1. Start client(s). You can start multiple clients.
   - For GrpcWpfSample.Client.Core.exe, write a name and hit enter key.
1. On client, write a message and hit enter key.
1. Your message will be registered on server, and you can see it on the client.
