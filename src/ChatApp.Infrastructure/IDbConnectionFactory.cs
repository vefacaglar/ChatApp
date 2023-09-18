using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace ChatApp.Infrastructure
{
    public interface IDbConnectionFactory
    {
        IDbConnection CreateConnection();
    }

    public interface IMsSqlDbConnectionFactory : IDbConnectionFactory
    {
    }

    public class MsSqlConnectionFactory : IMsSqlDbConnectionFactory
    {
        private readonly IConfiguration _configuration;

        public MsSqlConnectionFactory(
            IConfiguration configuration
            )
        {
            _configuration = configuration;
        }

        public IDbConnection CreateConnection()
        {
            return new SqlConnection(_configuration["ConnectionStrings:ChatDbRead"]);
        }
    }
}
