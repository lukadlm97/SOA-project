using AutoMapper;
using Grpc.Core;
using Grpc.Net.Client;
using Master.SOA.AuthGrpcApi.Models.Domain;
using Master.SOA.AuthGrpcApi.Services.Contracts;
using Master.SOA.GrpcProtoLibrary.Protos.Auth;
using Master.SOA.GrpcProtoLibrary.Protos.Interprocess;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Master.SOA.AuthGrpcApi.GrpcApi
{
    public class AuthGrpcService : Auth.AuthBase
    {
        private readonly IAuthService _service;
        private readonly IMapper _mapper;
        private readonly ILogger<AuthGrpcService> _logger;
        private readonly InterprocessCommunication.InterprocessCommunicationClient _client;

        public AuthGrpcService(IMapper mapper, IAuthService service, ILogger<AuthGrpcService> loggger)
        {
            (_mapper, _service, _logger) = (mapper, service, loggger);
            GrpcChannel channel = GrpcChannel.ForAddress("https://localhost:45679");
            _client = new InterprocessCommunication.InterprocessCommunicationClient(channel);
        }


        public async override Task<ChangeRoleReply> UpdateUserRole(UpdateRoleRequest request, ServerCallContext context)
        {
            var isChanged = await _service.ChangeRole(request.AdminUsername, request.Username, request.Role);

            if (isChanged)
            {
                return new ChangeRoleReply
                {
                    Code = GrpcProtoLibrary.Protos.Auth.StatusCode.Success,
                    Message = "Role of user changed successfully"
                };
            }

            return new ChangeRoleReply
            {
                Code = GrpcProtoLibrary.Protos.Auth.StatusCode.Error,
                Message = $"Role of user cann't be changed"
            };
        }

        public async override Task<LogInReply> LogIn(LogInRequest request, ServerCallContext context)
        {
            var user = _mapper.Map<User>(request);

            var token = await _service.LogIn(user);

            if (token == null)
            {
                return new LogInReply
                {
                    Code = GrpcProtoLibrary.Protos.Auth.StatusCode.Error,
                    Role = null
                };
            }

            var guidId = Guid.NewGuid().ToString();

            await _client.SayGuidAsync(new HelloRequest { Guid = guidId, Role = token });

            return new LogInReply
            {
                Code = GrpcProtoLibrary.Protos.Auth.StatusCode.Success,
                Role = token
            };
        }

        public async override Task<RegisterReply> RegisterUser(RegisterRequest request, ServerCallContext context)
        {
            var isCreated = await _service.Register(_mapper.Map<User>(request));

            return isCreated ?
             new RegisterReply
             {
                 Code = GrpcProtoLibrary.Protos.Auth.StatusCode.Success,
                 Message = "user successfully created!!!"
             }
             :
             new RegisterReply
             {
                 Code = GrpcProtoLibrary.Protos.Auth.StatusCode.Error,
                 Message = $"user cann't be created!!!"
             };
        }
    }
}