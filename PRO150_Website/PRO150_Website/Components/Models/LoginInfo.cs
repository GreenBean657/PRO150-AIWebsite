using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace PRO150_Website.Components.Models {
    public class LoginInfo {
        [Required(ErrorMessage = "Email Required")]
        [EmailAddress(ErrorMessage = "Invalid Email")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password Required")]
        public string Password { get; set; } = string.Empty;
        
        public async Task<bool> ValidateLoginAsync(IConfiguration configuration) {
            var connStr = configuration.GetConnectionString("DefaultConnection");
            if (string.IsNullOrWhiteSpace(connStr))
            {
                Console.WriteLine("[DEBUG] No connection string named 'DefaultConnection' found.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
            {
                Console.WriteLine("[DEBUG] Email or password missing (Email: '{0}', Password: '{1}')", Email, Password);
                return false;
            }

            await using var conn = new NpgsqlConnection(connStr);
            await conn.OpenAsync();

            // Parameterized to prevent SQL injection
            const string sql = @"SELECT ""userpassword""
                                FROM ""User""
                                WHERE LOWER(""email"") = LOWER(@email)
                                LIMIT 1;";

            await using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("email", Email.Trim());

            var result = await cmd.ExecuteScalarAsync();

            // Debugging output
            Console.WriteLine($"[DEBUG] Email query: {Email}");
            Console.WriteLine($"[DEBUG] Found user: {result != null}");
            Console.WriteLine($"[DEBUG] Stored password: '{result}'");
            Console.WriteLine($"[DEBUG] Input password: '{Password}'");
            Console.WriteLine($"[DEBUG] Match: {result?.ToString() == Password}");

            if (result is not string storedPassword)
                return false;

            return storedPassword == Password.Trim();
        }
    }

    public class RegisterInfo {
        [Required(ErrorMessage = "Email Required")]
        [EmailAddress(ErrorMessage = "Invalid Email")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password Required")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Confirm Password Required")]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; } = string.Empty;

        public async Task<bool> RegisterAsync(IConfiguration configuration)
        {
            var connStr = configuration.GetConnectionString("DefaultConnection");
            if (string.IsNullOrWhiteSpace(connStr))
            {
                Console.WriteLine("[DEBUG] No connection string named 'DefaultConnection' found.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
            {
                Console.WriteLine("[DEBUG] Email or password missing.");
                return false;
            }

            await using var conn = new NpgsqlConnection(connStr);
            await conn.OpenAsync();

            // Check if email already exists
            const string checkSql = @"SELECT COUNT(*) FROM ""User"" WHERE LOWER(""email"") = LOWER(@email);";
            await using var checkCmd = new NpgsqlCommand(checkSql, conn);
            checkCmd.Parameters.AddWithValue("email", Email.Trim());

            var count = (long?)await checkCmd.ExecuteScalarAsync() ?? 0;
            if (count > 0)
            {
                Console.WriteLine("[DEBUG] Email already exists.");
                return false;
            }

            // Insert new user
            const string insertSql = @"INSERT INTO ""User"" (""email"", ""userpassword"") VALUES (@email, @password);";
            await using var insertCmd = new NpgsqlCommand(insertSql, conn);
            insertCmd.Parameters.AddWithValue("email", Email.Trim());
            insertCmd.Parameters.AddWithValue("password", Password.Trim());

            try
            {
                await insertCmd.ExecuteNonQueryAsync();
                Console.WriteLine("[DEBUG] User registered successfully.");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[DEBUG] Registration error: {ex.Message}");
                return false;
            }
        }
    }
}