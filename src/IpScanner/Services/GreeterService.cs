using Grpc.Core;

namespace IpScanner.Services;

public class GreeterService : Greeter.GreeterBase
{
    public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context) =>
        Task.FromResult(new HelloReply
        {
            Message = $"Hello {request.Name}",
        });
}