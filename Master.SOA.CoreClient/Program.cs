using Grpc.Net.Client;
using Master.SOA.GrpcProtoLibrary.Protos.Greet;
using System;
using System.Threading.Tasks;

namespace Master.SOA.CoreClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            GrpcChannel channel = GrpcChannel.ForAddress("https://localhost:45679");
            var httpsClient = new Greeter.GreeterClient(channel);

            var reply = httpsClient.SayHello(new HelloRequest { Name = "Luka" });

            Console.WriteLine(reply.Message);

            Console.WriteLine("Press any key to close app...");
            Console.ReadLine();
        }
    }
}
