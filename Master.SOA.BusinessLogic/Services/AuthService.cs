using Master.SOA.BusinessLogic.Contracts;
using System.Threading.Tasks;

namespace Master.SOA.BusinessLogic.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _repository;

        public AuthService(IAuthRepository repository)
        => (_repository) = (repository);
        public async Task<string> GetRole(string guidId)
        {
            return await _repository.GetRole(guidId);
        }

        public async Task<bool> SetParams(string guidId, string role)
        {
            return await _repository.AddGuid(guidId, role);
        }
    }
}