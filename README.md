# GrpcWpfSample

Chat server-client applications as a sample implementation of gRPC on C# WPF. Read the [official quick start](https://grpc.io/docs/quickstart/csharp.html) beforehand.

- *Simple RPC*: Client registers a message to server.
- *Server-side streaming RPC*: Server streams messages to clients.

##### Hexagonal architecture

![GrpcWpfSample_archtecture](https://github.com/cactuaroid/GrpcWpfSample/blob/master/GrpcWpfSample_archtecture.png)

Note that  DB is not implemented but on-memory `List` is used as a placeholder.


# How to run

Start GrpcWpfSample.Server.exe first, then start GrpcWpfSample.Client.exe, write a message and hit enter key. Your message will be registered on server, and you can see it on the client. You can start multiple clients.
