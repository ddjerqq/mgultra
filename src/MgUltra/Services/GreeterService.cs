using Grpc.Net.Client;
using IpScanner;

namespace MgUltra.Services;

public sealed class GreeterService
{
    private readonly Greeter.GreeterClient _client;

    public GreeterService()
    {
        var channel = GrpcChannel.ForAddress("http://127.0.0.1:5000");
        _client = new Greeter.GreeterClient(channel);
    }

    public async Task<HelloReply> SayHello(HelloRequest request) =>
        await _client.SayHelloAsync(request);
}