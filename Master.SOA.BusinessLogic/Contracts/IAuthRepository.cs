using System.Threading.Tasks;

namespace Master.SOA.BusinessLogic.Contracts
{
    public interface IAuthRepository
    {
        Task<bool> AddGuid(string guidId, string role);
        Task<string> GetRole(string guidId);
    }
}