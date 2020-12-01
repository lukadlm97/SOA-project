using Dapper;
using Master.SOA.BusinessLogic.Contracts;
using Master.SOA.Domain.Configurations;
using Master.SOA.Domain.DataTransferObjects;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
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
            var sql = @"insert into [Master.SOA].[dbo].[Tick]([Time],[OpenPrice],[ClosePrice],[HighPrice],[LowPrice],[InstrumentId])
                        values(@Time,@OpenPrice, @ClosePrice, @HighPrice,@LowPrice, @InstrumentId)
                        SELECT @Id=@@IDENTITY";

            var dynamicParamteras = new DynamicParameters();
            dynamicParamteras.Add("@Id", 0, System.Data.DbType.Int32, ParameterDirection.Output);
            dynamicParamteras.Add("@Time", DateTime.Now);
            dynamicParamteras.Add("@OpenPrice", obj.OpenPrice);
            dynamicParamteras.Add("@ClosePrice", obj.ClosePrice);
            dynamicParamteras.Add("@HighPrice", obj.HighPrice);
            dynamicParamteras.Add("@LowPrice", obj.LowPrice);
            dynamicParamteras.Add("@InstrumentId", obj.Instrument.Id);

            try
            {
                await using var connection = new SqlConnection(_options.Value.ConnectionString);

                await connection.ExecuteAsync(sql, dynamicParamteras);
                var newIdentity = dynamicParamteras.Get<int>("@Id");

                _logger.LogInformation($"Quota with id={newIdentity} inserted");

                return true;
            }
            catch (Exception)
            {
                _logger.LogError("Error occurred while inserting data to database");
            }
            return false;

        }

        public async Task<bool> Delete(int id)
        {
            var sql = "DELETE FROM [Master.SOA].[dbo].[Tick] WHERE Id=@Id";
            var parametar = new { Id = id };

            try
            {
                await using var connection = new SqlConnection(_options.Value.ConnectionString);

                var result = await connection.ExecuteAsync(sql, parametar);
                _logger.LogInformation("Rows affected by deletion :" + result);

                if (result == 0)
                    return false;

                return true;
            }
            catch (Exception)
            {
                _logger.LogError("Error occurred while deleting data from database");
            }
            return false;
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

        public async Task<bool> Update(int id, TickDto obj)
        {
            var sql = @"UPDATE [Master.SOA].[dbo].[Tick]
                        SET OpenPrice = @openValue, ClosePrice = @closeValue, HighPrice = @highValue, LowPrice = @lowValue,SymbolId=@symbolValue
                        WHERE Id = @Id";

            var parametars = new DynamicParameters();
            parametars.Add("@Id", id);
            parametars.Add("@openValue", obj.OpenPrice);
            parametars.Add("@closeValue", obj.ClosePrice);
            parametars.Add("@highValue", obj.HighPrice);
            parametars.Add("@lowValue", obj.LowPrice);
            parametars.Add("@symbolValue", obj.Instrument.Id);

            try
            {
                await using var connection = new SqlConnection(_options.Value.ConnectionString);

                var result = await connection.ExecuteAsync(sql, parametars);

                _logger.LogInformation($"Rows affected by update :" + result);

                if (result == 0)
                    return false;

                return true;
            }
            catch (Exception)
            {
                _logger.LogError("Error occurred while update data from database");
            }
            return false;
        }
    }
}