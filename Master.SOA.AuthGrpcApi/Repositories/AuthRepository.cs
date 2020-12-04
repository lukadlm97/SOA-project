using Master.SOA.AuthGrpcApi.Models.Configurations;
using Master.SOA.AuthGrpcApi.Models.Dbo;
using Master.SOA.AuthGrpcApi.Repositories.Contracts;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Master.SOA.AuthGrpcApi.Repositories
{
    public class AuthRepository : IAuthRepository<UserDbo>
    {
        private readonly ILogger<AuthRepository> _logger;
        private readonly IOptions<ConfigSettings> _options;

        public AuthRepository(IOptions<ConfigSettings> options, ILogger<AuthRepository> logger)
           => (_options, _logger) = (options, logger);
        public Task<bool> Create(UserDbo obj)
        {
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable<UserDbo>> GetAll()
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> Update(int id, UserDbo obj)
        {
            throw new System.NotImplementedException();
        }
    }
}