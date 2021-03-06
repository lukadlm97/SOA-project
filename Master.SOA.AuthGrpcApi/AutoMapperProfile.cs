﻿using AutoMapper;
using Master.SOA.AuthGrpcApi.Models.Dbo;
using Master.SOA.AuthGrpcApi.Models.Domain;
using Master.SOA.GrpcProtoLibrary.Protos.Auth;

namespace Master.SOA.AuthGrpcApi
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, UserDbo>()
                .ReverseMap();
            CreateMap<Role, RoleDbo>()
               .ReverseMap();
            CreateMap<LogInRequest, User>();
            CreateMap<RegisterRequest, User>();
            CreateMap<UpdateRoleRequest, User>()
                .ForPath(dest => dest.Role.Name,opt => opt.MapFrom(src=>src.Role));

        }

    }
}