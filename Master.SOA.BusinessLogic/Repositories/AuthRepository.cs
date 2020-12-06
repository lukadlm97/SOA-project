using Dapper;
using Master.SOA.BusinessLogic.Contracts;
using Master.SOA.Domain.Configurations;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Master.SOA.BusinessLogic.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly ILogger<AuthRepository> _logger;
        private readonly IOptions<ConfigSettings> _options;

        public AuthRepository(IOptions<ConfigSettings> options, ILogger<AuthRepository> logger)
           => (_options, _logger) = (options, logger);

        public async Task<bool> AddGuid(string guidId, string role)
        {
            var sql = @"insert into [Master.SOA].[dbo].[AuthState] values(@GuidId,@Role) select @Id = @@IDENTITY";

            var dynamicParams = new DynamicParameters();
            dynamicParams.Add("@Id", 0, System.Data.DbType.Int32, ParameterDirection.Output);
            dynamicParams.Add("@GuidId", guidId);
            dynamicParams.Add("@Role", role);

            try
            {
                await using var connection = new SqlConnection(_options.Value.ConnectionString);

                await connection.ExecuteAsync(sql, dynamicParams);
                var newIdentity = dynamicParams.Get<int>("@Id");

                _logger.LogInformation($"state with id={newIdentity} inserted");


                return true;
            }
            catch (Exception)
            {
                _logger.LogError("Problem with inserting new auth entity");
                return false;
            }
        }

        public async Task<string> GetRole(string guidId)
        {
            var sql = @"SELECT TOP (1000)[Role]
                          FROM [Master.SOA].[dbo].[AuthState]
                          WHERE [GuidId] like '%@Param%'";

            var dynamicParams = new DynamicParameters();
            dynamicParams.Add("@Param", guidId);

            try
            {
                await using var connection = new SqlConnection(_options.Value.ConnectionString);

                var result = await connection.QueryFirstOrDefaultAsync(sql, dynamicParams);
                _logger.LogInformation("Data retrieved from database");

                return result;
            }
            catch (Exception)
            {
                _logger.LogError("Problem with getting auth entity");
                return null;
            }
        }
    }
}