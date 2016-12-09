using System.Collections.Generic;
using System.Linq;
using Dapper;
using System.Data;
using MySql.Data.MySqlClient;
using theWall.Models;
using Microsoft.Extensions.Options;
namespace theWall.Factory
{
    public class CommentFactory : IFactory<Message>
    {
        private readonly IOptions<MySqlOptions> mysqlConfig;
        public CommentFactory(IOptions<MySqlOptions> conf)
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
        public IEnumerable<Comment> GetComments()
        {
            using (IDbConnection dbConnection = Connection)
            {
                dbConnection.Open();
                var CommentQuery = $"SELECT * FROM comments JOIN users ON comments.user_id WHERE users.id = comments.user_id";
                return dbConnection.Query<Comment, User, Comment>(CommentQuery, (comment, user) =>
                {
                    comment.user = user;
                    return comment;
                });
            }
        }
        public void Add(Comment comment, int UserId)
        {
            using (IDbConnection dbConnection = Connection)
            {
                string query = $"INSERT INTO comments (content, user_id, created_at, updated_at, message_id) VALUES (@content, '{UserId}', NOW(), NOW(), @message_id)";
                dbConnection.Open();
                dbConnection.Execute(query, comment);
            }
        }
    }
}