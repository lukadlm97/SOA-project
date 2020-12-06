using AutoMapper;
using Google.Protobuf.Collections;
using Grpc.Core;
using Master.SOA.BusinessLogic.Contracts;
using Master.SOA.Domain.Models;
using Master.SOA.GrpcProtoLibrary.Protos.Interprocess;
using Master.SOA.GrpcProtoLibrary.Protos.Ticker;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Master.SOA.TickGrpcApi.Services
{
    public class TickerService : Ticker.TickerBase
    {
        private readonly IDataService<Tick> _service;
        private readonly IMapper _mapper;
        private readonly ILogger<TickerService> _logger;
        private readonly IInstrumentService _instrumentService;

        public TickerService(IMapper mapper, IDataService<Tick> service, IInstrumentService instrumentService,
            ILogger<TickerService> loggger, IMemoryCache memoryCache)
        {
            (_mapper, _service, _instrumentService, _logger) = (mapper, service, instrumentService, loggger);
            
        }

        public override async Task<TickReply> GetTick(TickSearchRequest request, ServerCallContext context)
        {
            _logger.LogInformation($"Request GetQuote for quote for id={request.TickId} received !!!");

            var tick = await _service.GetById(request.TickId);

            if (tick == null)
                return new TickReply { Code = GrpcProtoLibrary.Protos.Ticker.StatusCode.Error };

            return _mapper.Map<TickReply>(tick);
        }

        public override async Task<MultipleTicksReply> GetTicks(MultipleTicksRequest request, ServerCallContext context)
        {
            _logger.LogInformation("Request GetQuotes received !!!");

            IEnumerable<Tick> ticks = null;

            switch (request.RequestCase)
            {
                case MultipleTicksRequest.RequestOneofCase.EmptyRequest:
                    ticks = await _service.GetAll(null);
                    break;

                case MultipleTicksRequest.RequestOneofCase.TicksRequested:
                    ticks = await _service.GetAll(request.TicksRequested.Count);
                    break;

                default:
                    ticks = null;
                    break;
            }
            if (ticks == null)
                return new MultipleTicksReply { Code = GrpcProtoLibrary.Protos.Ticker.StatusCode.Error };

            var destinationTicks = ticks.Select(x => _mapper.Map<TickReply>(x));

            return new MultipleTicksReply
            {
                Code = GrpcProtoLibrary.Protos.Ticker.StatusCode.Success,
                Ticks =
                {
                    _mapper.Map<RepeatedField<TickReply>>(destinationTicks)
                }
            };
        }

        public async override Task<TickChangesReply> CreateTick(TickToAdd request, ServerCallContext context)
        {
            _logger.LogInformation($"Request CreateTick received !!!");
            var isCreated = await _service.Create(_mapper.Map<Tick>(request));

            if (isCreated)
                return new TickChangesReply
                {
                    Code = GrpcProtoLibrary.Protos.Ticker.StatusCode.Success,
                    Message = "New tick successfully created"
                };
            else
                return new TickChangesReply
                {
                    Code = GrpcProtoLibrary.Protos.Ticker.StatusCode.Error,
                    Message = "Cannot create new quota"
                };
        }

        public async override Task<TickChangesReply> DeleteTick(TickSearchRequest request, ServerCallContext context)
        {
            _logger.LogInformation($"Request DeleteTick received !!!");
            var isDeleted = await _service.Delete(request.TickId);

            if (isDeleted)
                return new TickChangesReply
                {
                    Code = GrpcProtoLibrary.Protos.Ticker.StatusCode.Success,
                    Message = request.TickId + " was successfully deleted"
                };
            else
                return new TickChangesReply
                {
                    Code = GrpcProtoLibrary.Protos.Ticker.StatusCode.Error,
                    Message = request.TickId + " cannot be deleted"
                };
        }

        public async override Task<TickChangesReply> UpdateTick(TickToAdd request, ServerCallContext context)
        {
            _logger.LogInformation($"Request UpdateTick received !!!");
            var isUpdated = await _service.Update(request.InstrumentId, _mapper.Map<Tick>(request));

            if (isUpdated)
                return new TickChangesReply
                {
                    Code = GrpcProtoLibrary.Protos.Ticker.StatusCode.Success,
                    Message = "tick successsfully updated"
                };
            else
                return new TickChangesReply
                {
                    Code = GrpcProtoLibrary.Protos.Ticker.StatusCode.Error,
                    Message = "tick cann\'t be updated"
                };
        }

        public async override Task GetStreamOfTicks(EmptyRequest request, IServerStreamWriter<TickReply> responseStream, ServerCallContext context)
        {
            _logger.LogInformation($"Request StreamQuotes received !!!");

            var ticks = await _service.GetAll(null);

            foreach (var tick in ticks)
            {
                await responseStream.WriteAsync(_mapper.Map<TickReply>(tick));
            }
        }

        public async override Task<MultipleTicksReply> GetTickForSymbol(SymbolSearchRequest request, ServerCallContext context)
        {
            _logger.LogInformation($"Request GetTickForSymbol received !!!");

            var ticks = await _instrumentService.GetTicks(request.SymbolId);

            if (ticks == null)
                return new MultipleTicksReply
                {
                    Code = GrpcProtoLibrary.Protos.Ticker.StatusCode.Error
                };

            var destinationTicks = ticks.Select(x => _mapper.Map<TickReply>(x));

            return new MultipleTicksReply
            {
                Code = GrpcProtoLibrary.Protos.Ticker.StatusCode.Success,
                Ticks =
                {
                    _mapper.Map<RepeatedField<TickReply>>(destinationTicks)
                }
            };
        }

        public async override Task<TickChangesReply> ClientStreaming(IAsyncStreamReader<TickToAdd> requestStream, ServerCallContext context)
        {
            bool isAllCreated = true;

            await foreach (var request in requestStream.ReadAllAsync())
            {
                _logger.LogInformation($"import new tick for symbole id={request.Symbol}");

                isAllCreated = await _service.Create(_mapper.Map<Tick>(request));
            }

            if (isAllCreated)
                return new TickChangesReply
                {
                    Code = GrpcProtoLibrary.Protos.Ticker.StatusCode.Success,
                    Message = "new ticks created"
                };
            else
                return new TickChangesReply
                {
                    Code = GrpcProtoLibrary.Protos.Ticker.StatusCode.Error,
                    Message = "new ticks doesn\'t created"
                };
        }

        
    }
}