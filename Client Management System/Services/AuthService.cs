using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;

namespace Client_Management_System.Services
{
    public class AuthService
    {
        private readonly string _connection;

        public AuthService(IConfiguration config)
        {
            _connection = config.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string not found.");
        }
        public (bool success, string message, string role, int userId) Login(string usernameInput, string passwordInput)
        {
            using SqlConnection con = new SqlConnection(_connection);
            con.Open();

            string query = @"SELECT Id, Username, PasswordHash, Role, FailedAttempts, IsLocked 
                             FROM LoginUser 
                             WHERE Username = @username";

            using SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@username", usernameInput);

            using SqlDataReader reader = cmd.ExecuteReader();

            if (!reader.Read())
                return (false, "User not found", "", 0);

            int userId = reader["Id"] == DBNull.Value ? 0 : Convert.ToInt32(reader["Id"]);
            string hashedPassword = reader["PasswordHash"]?.ToString() ?? "";
            string role = reader["Role"]?.ToString() ?? "";

            int failedAttempts = reader["FailedAttempts"] == DBNull.Value ? 0 : Convert.ToInt32(reader["FailedAttempts"]);
            bool isLocked = reader["IsLocked"] != DBNull.Value && Convert.ToBoolean(reader["IsLocked"]);

            reader.Close();

            if (isLocked)
                return (false, "Account is locked", "", 0);

            bool isValid = BCrypt.Net.BCrypt.Verify(passwordInput, hashedPassword);

            if (!isValid)
            {
                failedAttempts++;

                if (failedAttempts >= 3)
                {
                    string lockQuery = @"UPDATE LoginUser 
                                         SET FailedAttempts=@fa, IsLocked=1 
                                         WHERE Id=@id";

                    using SqlCommand lockCmd = new SqlCommand(lockQuery, con);
                    lockCmd.Parameters.AddWithValue("@fa", failedAttempts);
                    lockCmd.Parameters.AddWithValue("@id", userId);
                    lockCmd.ExecuteNonQuery();

                    return (false, "Account locked", "", 0);
                }

                string failQuery = @"UPDATE LoginUser 
                                     SET FailedAttempts=@fa 
                                     WHERE Id=@id";

                using SqlCommand failCmd = new SqlCommand(failQuery, con);
                failCmd.Parameters.AddWithValue("@fa", failedAttempts);
                failCmd.Parameters.AddWithValue("@id", userId);
                failCmd.ExecuteNonQuery();

                return (false, $"Invalid password ({failedAttempts}/3)", "", 0);
            }

            string resetQuery = @"UPDATE LoginUser 
                                  SET FailedAttempts=0 
                                  WHERE Id=@id";

            using SqlCommand resetCmd = new SqlCommand(resetQuery, con);
            resetCmd.Parameters.AddWithValue("@id", userId);
            resetCmd.ExecuteNonQuery();

            return (true, "Login successful", role, userId);
        }
    }
}