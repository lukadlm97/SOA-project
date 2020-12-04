using AutoMapper;
using Master.SOA.AuthGrpcApi.Models.Dbo;
using Master.SOA.AuthGrpcApi.Models.Domain;
using Master.SOA.AuthGrpcApi.Repositories.Contracts;
using Master.SOA.AuthGrpcApi.Services.Contracts;
using System;
using System.Threading.Tasks;

namespace Master.SOA.AuthGrpcApi.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository<UserDbo> _repository;
        private readonly IMapper _mapper;

        public AuthService(IAuthRepository<UserDbo> repository, IMapper mapper)
        => (_repository, _mapper) = (repository, mapper);

        public Task<bool> ChangeRole(string adminName, string username, string role)
        {
            throw new NotImplementedException();
        }

        public Task<string> LogIn(User obj)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Register(int id, User obj)
        {
            throw new NotImplementedException();
        }
    }
}