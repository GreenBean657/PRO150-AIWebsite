using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace PRO150_Website.Components.Models
{
    public class LoginInfo
    {
        [Required(ErrorMessage = "Email Required")]
        [EmailAddress(ErrorMessage = "Invalid Email")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password Required")]
        public string Password { get; set; } = string.Empty;
        
        public async Task<bool> ValidateLoginAsync(IConfiguration configuration)
        {
            var connStr = configuration.GetConnectionString("DefaultConnection");
            if (string.IsNullOrWhiteSpace(connStr))
                return false;

            if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
                return false;

            await using var conn = new NpgsqlConnection(connStr);
            await conn.OpenAsync();

            // Parameterized to prevent SQL injection
            const string sql = @"SELECT ""UserPassword""
                                FROM ""User""
                                WHERE ""Email"" = @email
                                LIMIT 1;";

            await using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("email", Email);

            var result = await cmd.ExecuteScalarAsync();

            if (result is not string storedPassword)
                return false;

            return storedPassword == Password;
        }
    }
}