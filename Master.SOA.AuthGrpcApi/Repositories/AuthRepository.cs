using Dapper;
using Master.SOA.AuthGrpcApi.Models.Configurations;
using Master.SOA.AuthGrpcApi.Models.Dbo;
using Master.SOA.AuthGrpcApi.Repositories.Contracts;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Master.SOA.AuthGrpcApi.Repositories
{
    public class AuthRepository : IAuthRepository<UserDbo>
    {
        private readonly ILogger<AuthRepository> _logger;
        private readonly IOptions<ConfigSettings> _options;

        public AuthRepository(IOptions<ConfigSettings> options, ILogger<AuthRepository> logger)
           => (_options, _logger) = (options, logger);

        public async Task<bool> Create(UserDbo obj)
        {
            obj.Role=new RoleDbo { Id=2};

            var sql = @"insert into [Master.SOA.Auth].[dbo].[Users]
                        ([Username],[Password],[FirstName],[LastName],[RegisterDate],[RoleId])
                        values(@Username,@Password,@FirstName,@LastName,@RegisterDate,@RoleId)
                        select @Id=@@IDENTITY";

            var dynamicParamteras = new DynamicParameters();
            dynamicParamteras.Add("@Id", 0, System.Data.DbType.Int32, ParameterDirection.Output);
            dynamicParamteras.Add("@Username", obj.Username);
            dynamicParamteras.Add("@Password", obj.Password);
            dynamicParamteras.Add("@FirstName", obj.FirstName);
            dynamicParamteras.Add("@LastName", obj.LastName);
            dynamicParamteras.Add("@RegisterDate", DateTime.Now);
            dynamicParamteras.Add("@RoleId", obj.Role.Id);
            
            try
            {
                await using var connection = new SqlConnection(_options.Value.ConnectionString);

                await connection.ExecuteAsync(sql, dynamicParamteras);
                var newIdentity = dynamicParamteras.Get<int>("@Id");

                _logger.LogInformation($"User with id= {newIdentity} inserted!!!");
                
                return true;
            }
            catch (Exception)
            {
                _logger.LogError("Error occurred while inserting data to database");
            }

            return false;
        }

        public async Task<IEnumerable<UserDbo>> GetAll()
        {
            var roles = await GetRoles();

            var sql = @"SELECT TOP (1000) u.*,r.*
                        FROM [Master.SOA.Auth].[dbo].[Users] u 
                        LEFT JOIN [Master.SOA.Auth].[dbo].[Roles] r 
                        ON(u.RoleId= r.Id)";

            try
            {
                await using var connection = new SqlConnection(_options.Value.ConnectionString);
                var result = await connection.QueryAsync<UserDbo, RoleDbo, UserDbo>(sql,
                    (user, role)=> { user.Role = role;return user; });

                _logger.LogInformation("Data retrieved from database");

                return result;
            }
            catch (Exception e)
            {
                _logger.LogError("Error occurred while retrieving data from database"+e.Message);
                return null;
            }
        }

        public async Task<bool> Update(int id, UserDbo obj)
        {
            var roles = await GetRoles();

            var role = roles.FirstOrDefault(x => x.Name == obj.Role.Name)?.Id;

            if (role == null)
                obj.Role.Id = 2;
            else
                obj.Role.Id = (int)role;

            var sql = @"update [Master.SOA.Auth].[dbo].[Users]
                        set [RoleId] = @RoleId
                        where [Id] = @UserId";

            var parametars = new DynamicParameters();
            parametars.Add("@UserId", id);
            parametars.Add("@RoleId", obj.Role.Id);

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

        private async Task<IEnumerable<RoleDbo>> GetRoles()
        {
            var sql = "select* from [Master.SOA.Auth].[dbo].[Roles]";

            try
            {
                await using var connection = new SqlConnection(_options.Value.ConnectionString);

                var result = await connection.QueryAsync<RoleDbo>(sql);

                return result;
            }
            catch (Exception)
            {
                _logger.LogError("Error occurred while inserting data to database");
                return null;
            }
        }
    }
}