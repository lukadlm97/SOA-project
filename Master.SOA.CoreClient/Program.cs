using Grpc.Net.Client;
using Master.SOA.GrpcProtoLibrary.Protos.Greeter;
using Master.SOA.GrpcProtoLibrary.Protos.Ticker;
using System;
using System.Threading.Tasks;

namespace Master.SOA.CoreClient
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            GrpcChannel channel = GrpcChannel.ForAddress("https://localhost:45679");
            var httpsClient = new Ticker.TickerClient(channel);

            var reply = await httpsClient.GetTickAsync(new TickSearchRequest { TickId = 1 });

            Console.WriteLine(reply.Symbol);

            Console.WriteLine("Press any key to close app...");
            Console.ReadLine();
        }
    }
}