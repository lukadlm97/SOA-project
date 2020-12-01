using Master.SOA.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Master.SOA.BusinessLogic.Contracts
{
    public interface IInstrumentService
    {
        Task<IEnumerable<Tick>> GetTicks(int symbolId);
    }
}