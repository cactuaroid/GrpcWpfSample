# GrpcWpfSample

This is a sample implementation of gRPC on WPF apps. Read the [official quick start](https://grpc.io/docs/quickstart/csharp.html) beforehand.

This is a chat server-client application, both sides are WPF, contains:
- Simple RPC: Client registers a message to server.
- Server-side streaming RPC: Server streams messages to clients.

![GrpcWpfSample_archtecture](https://github.com/cactuaroid/GrpcWpfSample/blob/master/GrpcWpfSample_archtecture.png)

# How to run

Start GrpcWpfSample.Server.exe first, then start GrpcWpfSample.Client.exe, write a message and hit enter key. Your message will be registered on server, and you can see it on the client. You can start multiple clients.
