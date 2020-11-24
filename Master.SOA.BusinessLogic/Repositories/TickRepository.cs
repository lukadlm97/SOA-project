using Master.SOA.BusinessLogic.Contracts;
using Master.SOA.Domain.Configurations;
using Master.SOA.Domain.DataTransferObjects;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Master.SOA.BusinessLogic.Repositories
{
    public class TickRepository : IDataRepository<TickDto>
    {
        private readonly ILogger<TickRepository> _logger;
        private readonly IOptions<ConfigSettings> _options;

        public TickRepository(IOptions<ConfigSettings> options, ILogger<TickRepository> logger)
           => (_options, _logger) = (options, logger);

        public Task<bool> Create(TickDto obj)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> Delete(int id)
        {
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable<TickDto>> GetAll()
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> Update(int id, TickDto obj)
        {
            throw new System.NotImplementedException();
        }
    }
}