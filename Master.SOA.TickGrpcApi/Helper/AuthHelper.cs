using Dapper;
using System;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Master.SOA.TickGrpcApi.Helper
{
    public static class AuthHelper
    {
        public  static string GetRole(string guidId)
        {
            var sql = @"SELECT TOP (1000)[Role]
                          FROM [Master.SOA].[dbo].[AuthState]
                          WHERE [GuidId] like '%@Param%'";

            var sql1 = @"SELECT TOP (1000) [Id]
                      ,[GuidId]
                      ,[Role]
                         FROM [Master.SOA].[dbo].[AuthState]"; 

            var dynamicParams = new DynamicParameters();
            dynamicParams.Add("@Param", guidId);

            try
            {
                using var connection = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=master;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");

                var result =  connection.QueryFirstOrDefault<string>(sql, dynamicParams);

                return result;
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}