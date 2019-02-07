using Dinky.Domain.ViewModels;
using Dapper;
using System;
using System.Threading.Tasks;

namespace Dinky.Services.service
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<User> SELECTAsync(string username);
        Task<User> SELECTAsync(int code);
    }

    public class UserRepository : SqlRepository<User>, IUserRepository
    {
        public UserRepository(string connectionString) : base(connectionString) { }
        
        public async Task<User> SELECTAsync(string username)
        {
            using (var conn = GetOpenConnection())
            {
                var sql = "SELECT * FROM users WHERE username = @username";
                var parameters = new DynamicParameters();
                parameters.Add("@username", username, System.Data.DbType.String);
                return await conn.QueryFirstOrDefaultAsync<User>(sql, parameters);
            }
        }
        
        public async Task<User> SELECTAsync(int code)
        {
            using (var conn = GetOpenConnection())
            {
                var sql = "SELECT * FROM users WHERE code = @code";
                var parameters = new DynamicParameters();
                parameters.Add("@code", code, System.Data.DbType.Int64);
                return await conn.QueryFirstOrDefaultAsync<User>(sql, parameters);
            }
        }
    }
}
