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

Start GrpcWpfSample.Server.Wpf.exe (WPF on .NET Framework) or GrpcWpfSample.Server.Core.exe (Console app on .NET Core) first, then start GrpcWpfSample.Client.exe, write a message and hit enter key. Your message will be registered on server, and you can see it on the client. You can start multiple clients.
