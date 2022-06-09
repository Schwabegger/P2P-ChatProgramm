// Copyright ©️ Schwabegger Moritz. All Rights Reserved
// Supporters:
// ඞ Hackl Tobias
// ඞ Ratzenböck Peter

using Grpc.Net.Client;
using GrpcShared;

Main();

static async Task Main()
{
    // Create channel.
    // Represents long-lived connection to gRPC service.
    // The port number(5001) must match the port of the gRPC server.
    // Tip: In ASP.NET Core apps, use client factory (similar to
    //      IHttpClientFactory (see https://docs.microsoft.com/en-us/aspnet/core/grpc/clientfactory).
    var channel = GrpcChannel.ForAddress("http://192.168.178.26:5000");


    //(string name, string pfp, long id) = await CreatePrivateChatroom("1.1.1.1", "mein name", "mein pfp", 5);
    
    await UnaryCall(channel);
    await channel.ShutdownAsync();
    Console.WriteLine("end");
}

static async Task UnaryCall(GrpcChannel channel)
{
    //var client = new Greeter.GreeterClient(channel);
    //var reply = await client.SayHelloAsync(new HelloRequest { Name = "GreeterClient" });
    var client = new Greeter.GreeterClient(channel);
    var reply = await client.RequestedUserPrivateAsync(new RequestUserMsg { Ip = "1.1.1.1" });
    Console.WriteLine(reply.Done);
}


Console.ReadKey();