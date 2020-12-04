using System.Collections.Generic;
using System.Threading.Tasks;

namespace Master.SOA.AuthGrpcApi.Repositories.Contracts
{
    public interface IAuthRepository<T>
    {
        Task<IEnumerable<T>> GetAll();
        Task<bool> Create(T obj);
        Task<bool> Update(int id, T obj);
    }
}