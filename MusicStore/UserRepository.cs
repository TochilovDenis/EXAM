using MusicStore;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System;
using MusicStore.Models;

public class UserRepository
{
    public async Task<User> GetUserAsync(string login)
    {
        using (var conn = Init_Conn.GetConnection())
        {
            await conn.OpenAsync();
            const string sql = "SELECT Id, Login, PasswordHash, Role, IsActive FROM Users WHERE Login = @Login";

            using (var cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@Login", login);
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        return new User
                        {
                            Id = reader.GetInt32(0),
                            Login = reader.GetString(1),
                            PasswordHash = reader.GetString(2),
                            Role = reader.GetString(3),
                            IsActive = reader.GetBoolean(4)
                        };
                    }
                }
            }
        }
        return null;
    }

    public async Task RegisterUserAsync(string login, string password, string role)
    {
        using (var conn = Init_Conn.GetConnection())
        {
            await conn.OpenAsync();
            const string sql = @"
                INSERT INTO Users (Login, PasswordHash, Role)
                VALUES (@Login, @PasswordHash, @Role)";

            using (var cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@Login", login);
                cmd.Parameters.AddWithValue("@PasswordHash",
                    Convert.ToBase64String(System.Security.Cryptography.SHA256.Create()
                    .ComputeHash(System.Text.Encoding.UTF8.GetBytes(password))));
                cmd.Parameters.AddWithValue("@Role", role);
                await cmd.ExecuteNonQueryAsync();
            }
        }
    }
}