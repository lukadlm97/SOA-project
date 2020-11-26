using Dapper;
using Master.SOA.BusinessLogic.Contracts;
using Master.SOA.Domain.Configurations;
using Master.SOA.Domain.DataTransferObjects;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Master.SOA.BusinessLogic.Repositories
{
    public class TickRepository : IDataRepository<TickDto>
    {
        private readonly ILogger<TickRepository> _logger;
        private readonly IOptions<ConfigSettings> _options;

        public TickRepository(IOptions<ConfigSettings> options, ILogger<TickRepository> logger)
           => (_options, _logger) = (options, logger);

        public async Task<bool> Create(TickDto obj)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> Delete(int id)
        {
            throw new System.NotImplementedException();
        }

        public async Task<IEnumerable<TickDto>> GetAll()
        {
            var sql = @"SELECT TOP (1000) t.*,i.*
                        FROM [Master.SOA].[dbo].[Tick] t 
                        left join [Master.SOA].[dbo].[Instrument] i 
                        on(t.InstrumentId=i.Id)";

            try
            {
                await using var connection = new SqlConnection(_options.Value.ConnectionString);
                var result = await connection.QueryAsync<TickDto, InstrumentDto, TickDto>(sql,
                                     (tick, instrument) => { tick.Instrument = instrument; return tick; });

                _logger.LogInformation("Data retrieved from database");

                return result;
            }
            catch (Exception)
            {
                _logger.LogError("Error occurred while retrieving data from database");
                return null;
            }
        }

        public Task<bool> Update(int id, TickDto obj)
        {
            throw new System.NotImplementedException();
        }
    }
}