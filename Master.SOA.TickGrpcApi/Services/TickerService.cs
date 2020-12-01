using AutoMapper;
using Google.Protobuf.Collections;
using Grpc.Core;
using Master.SOA.BusinessLogic.Contracts;
using Master.SOA.Domain.Models;
using Master.SOA.GrpcProtoLibrary.Protos.Ticker;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Master.SOA.TickGrpcApi.Services
{
    public class TickerService:Ticker.TickerBase
    {
        private readonly IDataService<Tick> _service;
        private readonly IMapper _mapper;
        private readonly ILogger<TickerService> _logger;

        public TickerService(IMapper mapper, IDataService<Tick> service,ILogger<TickerService> loggger)
        => (_mapper, _service, _logger) = (mapper, service,loggger);

        
        public override async Task<TickReply> GetTick(TickSearchRequest request, ServerCallContext context)
        {
            _logger.LogInformation($"Request GetQuote for quote for id={request.TickId} received !!!");

            var tick = await _service.GetById(request.TickId);

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

        public override Task<TickChangesReply> CreateTick(TickToAdd request, ServerCallContext context)
        {
            return base.CreateTick(request, context);
        }

    }
}
