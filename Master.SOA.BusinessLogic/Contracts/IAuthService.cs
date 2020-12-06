using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Master.SOA.BusinessLogic.Contracts
{
    public interface IAuthService
    {
        Task<bool> SetParams(string guidId, string role);
        Task<string> GetRole(string guidId);
    }
}
