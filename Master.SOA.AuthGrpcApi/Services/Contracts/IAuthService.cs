using Master.SOA.AuthGrpcApi.Models.Domain;
using System.Threading.Tasks;

namespace Master.SOA.AuthGrpcApi.Services.Contracts
{
    public interface IAuthService
    {
        Task<string> LogIn(User obj);
        Task<bool> Register(User obj);
        Task<bool> ChangeRole (string adminName, string username,string role);
    }
}