using Grpc.Core;
using Grpc.Core.Interceptors;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace Master.SOA.GrpcProtoLibrary.Interceptors.ServerSide
{
    public class HeaderLoggerInterceptor : Interceptor
    {
        private readonly ILogger<HeaderLoggerInterceptor> _logger;

        public HeaderLoggerInterceptor(ILogger<HeaderLoggerInterceptor> logger)
        => (_logger) = (logger);

        public override Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
            TRequest request,
            ServerCallContext context,
            UnaryServerMethod<TRequest, TResponse> continuation)
        {
            LogCall<TRequest, TResponse>(MethodType.Unary, context);

            return continuation(request, context);
        }

        public override Task ServerStreamingServerHandler<TRequest, TResponse>(TRequest request, IServerStreamWriter<TResponse> responseStream, ServerCallContext context, ServerStreamingServerMethod<TRequest, TResponse> continuation)
        {
            LogCall<TRequest, TResponse>(MethodType.ServerStreaming, context);

            return continuation(request, responseStream, context);
        }

        public override Task<TResponse> ClientStreamingServerHandler<TRequest, TResponse>(IAsyncStreamReader<TRequest> requestStream, ServerCallContext context, ClientStreamingServerMethod<TRequest, TResponse> continuation)
        {
            LogCall<TRequest, TResponse>(MethodType.ClientStreaming, context);

            return continuation(requestStream, context);
        }

        private void LogCall<TRequest, TResponse>(MethodType methodType, ServerCallContext context)
           where TRequest : class
           where TResponse : class
        {
            _logger.LogWarning($"Starting call. Type: {methodType}. Request: {typeof(TRequest)}. Response: {typeof(TResponse)}");
            WriteMetadata(context.RequestHeaders, "caller-user");
            WriteMetadata(context.RequestHeaders, "caller-machine");
            WriteMetadata(context.RequestHeaders, "caller-os");

            void WriteMetadata(Metadata headers, string key)
            {
                var headerValue = headers.SingleOrDefault(h => h.Key == key)?.Value;
                _logger.LogWarning($"{key}: {headerValue ?? "(unknown)"}");
            }
        }
    }
}