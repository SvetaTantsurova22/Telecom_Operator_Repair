using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;

namespace Telecom_Operator_Repair.Repository
{
    public class UserRepository
    {
        private readonly string connectionString;

        public UserRepository(string connString)
        {
            connectionString = connString;
        }

        public User AuthenticateUser(string username, string password, out string roleName)
        {
            roleName = null;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = @"SELECT u.*, r.RoleName 
                         FROM Users u
                         JOIN UserRoles r ON u.UserRole = r.ID
                         WHERE u.Username = @Username AND u.Password = @Password";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Username", username);
                cmd.Parameters.AddWithValue("@Password", password);

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    roleName = reader["RoleName"].ToString();

                    return new User
                    {
                        ID = (int)reader["ID"],
                        UserRole = (int)reader["UserRole"],
                        Username = reader["Username"].ToString(),
                        FullName = reader["FullName"].ToString(),
                        Email = reader["Email"].ToString(),
                        Phone = reader["Phone"].ToString(),
                        Password = reader["Password"].ToString()
                    };
                }
                return null;
            }
        }
    }
}
