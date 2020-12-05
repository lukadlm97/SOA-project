using Grpc.Core;
using Grpc.Net.Client;
using Master.SOA.CoreClient.Helpers;
using Master.SOA.GrpcProtoLibrary.Protos.Auth;
using Master.SOA.GrpcProtoLibrary.Protos.Greeter;
using Master.SOA.GrpcProtoLibrary.Protos.Ticker;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Master.SOA.CoreClient
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
           // GrpcChannel channel = GrpcChannel.ForAddress("https://localhost:45679");
           // var httpsClient = new Ticker.TickerClient(channel);

            GrpcChannel authChannel = GrpcChannel.ForAddress("https://localhost:56790");
            var httpsClient = new Auth.AuthClient(authChannel);

            var token = await httpsClient.UpdateUserRoleAsync(new UpdateRoleRequest
            {
                AdminUsername = "admin",
                Role = "PremiumUser",
                Username = "u1"
            });

            Console.WriteLine(token.Code+"   :   "+token.Message);
            /*await UpdateTickHandling(httpsClient,
                new TickToAdd
                {
                    InstrumentId = 1,
                    Close = (DecimalValue)5.6234m,
                    Open = (DecimalValue)5.6225m,
                    High = (DecimalValue)5.6238m,
                    Low = (DecimalValue)5.6224m,
                    Symbol = 2
                });
            await GetTicksForQuota(httpsClient, 1);*/
            //await ClientStreaming(httpsClient, 1);

            Console.WriteLine("Press any key to close app...");
            Console.ReadLine();
        }

        async static Task HandleCreateTick(Ticker.TickerClient client,TickToAdd tick)
        {
            var reply = await client.CreateTickAsync(tick);

            if(reply.Code == GrpcProtoLibrary.Protos.Ticker.StatusCode.Error)
            {
                Console.WriteLine("error occured while creating new tick");
                return;
            }

            Console.WriteLine(reply.Message);
        }

        async static Task HandleSingleCall(Ticker.TickerClient client,int id)
        {
            var request = new TickSearchRequest { TickId = id };
            var reply = await client.GetTickAsync(request);
            if(reply.Code == GrpcProtoLibrary.Protos.Ticker.StatusCode.Success)
                WriteSingleTick(reply);
            else
                Console.WriteLine("Error while retriving data from server!!!");
        }

        async static Task HandleUnaryCallWithMultiResponse(Ticker.TickerClient client,int? count = null)
        {
            MultipleTicksRequest request = null;

            if(count == null)
            {
                request = new MultipleTicksRequest { EmptyRequest = new EmptyRequest() };
            }
            else
            {
                request = new MultipleTicksRequest { TicksRequested = new TicksForRequest { Count = (int)count } };
            }

            var reply =await client.GetTicksAsync(request);

            if(reply.Code == GrpcProtoLibrary.Protos.Ticker.StatusCode.Error)
            {
                Console.WriteLine("Error occured while retriving data!!!");
                return;
            }    

            foreach(var tick in reply.Ticks)
            {
                WriteSingleTick(tick);
            }

        }

        async static Task ServerStreamingHandling(Ticker.TickerClient client)
        {
            using(var call = client.GetStreamOfTicks(new EmptyRequest()))
            {
                await foreach (var tick in call.ResponseStream.ReadAllAsync())
                {
                    WriteSingleTick(tick);
                }
            }
        }
            
        static async Task DeleteTickHandling(Ticker.TickerClient client,int id)
        {
            var request = new TickSearchRequest { TickId = id };

            var reply = await client.DeleteTickAsync(request);


            Console.WriteLine(reply.Message);
        }

        static async Task UpdateTickHandling(Ticker.TickerClient client,TickToAdd tickToAdd)
        {
            var reply = await client.UpdateTickAsync(tickToAdd);

            Console.WriteLine(reply.Message);
        }

        static async Task GetTicksForQuota(Ticker.TickerClient client,int symbolId)
        {
            var reply = await client.GetTickForSymbolAsync(new SymbolSearchRequest { SymbolId = symbolId });

            if(reply.Code == GrpcProtoLibrary.Protos.Ticker.StatusCode.Error)
            {
                Console.WriteLine("error occured while retriving data");
                return;
            }

            foreach(var tick in reply.Ticks)
            {
                WriteSingleTick(tick);
            }
        }

        static async Task ClientStreaming(Ticker.TickerClient client,int symbolId)
        {
            using(var call = client.ClientStreaming())
            {
                for(int i = 0; i < 5; i++)
                {
                    var request = GenerateRequest(symbolId);
                    await call.RequestStream.WriteAsync(request);
                    await Task.Delay(3000);
                }
                await call.RequestStream.CompleteAsync();

                var response = await call;

                Console.WriteLine(response.Message);
            }

        }
        public static TickToAdd GenerateRequest(int symbolId)
        {
            var array = TickResurseGenerator.CreateArray(4).AsEnumerable<double>();
            var high = array.Max<double>();
            var low = array.Min<double>();
            var open = TickResurseGenerator.GetRandomNumber(low, high);
            var close = TickResurseGenerator.GetRandomNumber(low, high);



            return new TickToAdd
            {
                Close = (DecimalValue)(decimal)close,
                Open = (DecimalValue)(decimal)open,
                Low = (DecimalValue)(decimal)low,
                High = (DecimalValue)(decimal)high,
                InstrumentId = 0,
                Symbol = symbolId
            };
        }


        private static void WriteSingleTick(TickReply reply)
        {
            decimal open = (decimal)reply.Open;

            var close = (decimal)reply.Close;
            var high = (decimal)reply.High;
            var low = (decimal)reply.Low;


            Console.WriteLine(reply.Symbol + " - " + reply.Time + "(" + open + "," + close + "," + high +
                                "," + low + ")");
        }
    }
}