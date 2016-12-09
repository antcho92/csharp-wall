using System.Collections.Generic;
using System.Linq;
using Dapper;
using System.Data;
using MySql.Data.MySqlClient;
using theWall.Models;
using Microsoft.Extensions.Options;
namespace theWall.Factory
{
    public class MessageFactory : IFactory<Message>
    {
        private readonly IOptions<MySqlOptions> mysqlConfig;
        public MessageFactory(IOptions<MySqlOptions> conf)
        {
            mysqlConfig = conf;
        }
        internal IDbConnection Connection
        {
            get {
                return new MySqlConnection(mysqlConfig.Value.ConnectionString);                
            }
        }
        public void AddMessage(Message message, int user_id)
        {
            using (IDbConnection dbConnection = Connection) {
                string query = $"INSERT INTO messages (content, user_id, created_at, updated_at) VALUES (@content, '{user_id}', NOW(), NOW())";
                dbConnection.Open();
                dbConnection.Execute(query, message);
            }
        }
        public IEnumerable<Message> GetMessages()
        {
            using (IDbConnection dbConnection = Connection)
            {
                dbConnection.Open();
                var MessageQuery = $"SELECT * FROM messages JOIN users ON messages.user_id WHERE users.id = messages.user_id";
                return dbConnection.Query<Message, User, Message>(MessageQuery, (message, user) =>
                {
                    message.user = user; 
                    return message; 
                });
            }
        }
        public void DeleteMessage(int MessageId, int UserId)
        {
            using (IDbConnection dbConnection = Connection)
            {
                dbConnection.Open();
                var query = $"DELETE FROM messages WHERE id={MessageId}";
                dbConnection.Execute(query);
            }
        }
    }
}