using AutoMapper;
using Google.Protobuf.WellKnownTypes;
using Master.SOA.Domain.DataTransferObjects;
using Master.SOA.Domain.Models;
using Master.SOA.GrpcProtoLibrary.Protos.Ticker;

namespace Master.SOA.TickGrpcApi
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Instrument, InstrumentDto>().ReverseMap();

            CreateMap<Tick,TickDto>()
                .ForMember(dest => dest.OpenPrice, opt => opt.MapFrom(src => src.Open))
                .ForMember(dest => dest.ClosePrice, opt => opt.MapFrom(src => src.Close))
                .ForMember(dest => dest.HighPrice, opt => opt.MapFrom(src => src.High))
                .ForMember(dest => dest.LowPrice, opt => opt.MapFrom(src => src.Low))
                .ReverseMap();

            CreateMap<Tick, TickReply>().ForMember(dest => dest.Open, opt => opt.MapFrom(src => (decimal)src.Open))
                .ForMember(dest => dest.Close, opt => opt.MapFrom(src => (decimal)src.Close))
                .ForMember(dest => dest.High, opt => opt.MapFrom(src => (decimal)src.High))
                .ForMember(dest => dest.Low, opt => opt.MapFrom(src => (decimal)src.Low))
                .ForMember(dest => dest.Symbol, opt => opt.MapFrom((src => src.Instrument.Name)))
                .ForMember(dest => dest.Time, opt => opt.MapFrom((src => Timestamp.FromDateTimeOffset(src.Time))));
        }
    }
}