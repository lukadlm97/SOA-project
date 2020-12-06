using AutoMapper;
using Master.SOA.AuthGrpcApi.Models.Dbo;
using Master.SOA.AuthGrpcApi.Models.Domain;
using Master.SOA.AuthGrpcApi.Repositories.Contracts;
using Master.SOA.AuthGrpcApi.Services.Contracts;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Master.SOA.AuthGrpcApi.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository<UserDbo> _repository;
        private readonly IMapper _mapper;

        public AuthService(IAuthRepository<UserDbo> repository, IMapper mapper)
        => (_repository, _mapper) = (repository, mapper);

        public async Task<bool> ChangeRole(string adminName, string username, string role)
        {
            var users = await _repository.GetAll();

            if (!users.Any(x => x.Username == adminName))
                return false;

            var id = users.FirstOrDefault(x => x.Username == username)?.Id;

            if (id == null)
                return false;

            var user = new User
            {
                Role = new Role
                {
                    Name = role
                }
            };

            return await _repository.Update((int)id, _mapper.Map<UserDbo>(user));
        }

        public async Task<string> LogIn(User obj)
        {
            var users = await _repository.GetAll();

            var isSuccess = users.Any(x => x.Username == obj.Username && x.Password == obj.Password);

            if (isSuccess)
            {
                return users.FirstOrDefault(x => x.Username == obj.Username && x.Password == obj.Password)?.Role.Name;
            }

            return null;
        }

        public async Task<bool> Register(User obj)
        {
            return await _repository.Create(_mapper.Map<UserDbo>(obj));
        }
    }
}