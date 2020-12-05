using System;

namespace Master.SOA.AuthGrpcApi.Models.Dbo
{
    public class UserDbo
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime RegisterDate { get; set; }
        public RoleDbo Role { get; set; }
    }
}