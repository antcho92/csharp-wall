using System.Collections.Generic;
using System.Linq;
using Dapper;
using System.Data;
using MySql.Data.MySqlClient;
using theWall.Models;
using Microsoft.Extensions.Options;
namespace theWall.Factory
{
    public class UserFactory : IFactory<User>
    {
        private readonly IOptions<MySqlOptions> mysqlConfig;
        public UserFactory(IOptions<MySqlOptions> conf)
        {
            mysqlConfig = conf;
        }
        internal IDbConnection Connection
        {
            get
            {
                return new MySqlConnection(mysqlConfig.Value.ConnectionString);
            }
        }
        public void Register(User user)
        {
            using (IDbConnection dbConnection = Connection) {
                string query = "INSERT INTO users (first_name, last_name, email, password, created_at, updated_at) VALUES (@first_name, @last_name, @email, @password, NOW(), NOW())";
                dbConnection.Open();
                dbConnection.Execute(query, user);
            }
        }
        public User Login(string email)
        {
            using (IDbConnection dbConnection = Connection) {
                string query = "SELECT * FROM users WHERE email=@Email";
                dbConnection.Open();
                return dbConnection.Query<User>(query, new {Email = email}).FirstOrDefault();
            }
        }
    }
}