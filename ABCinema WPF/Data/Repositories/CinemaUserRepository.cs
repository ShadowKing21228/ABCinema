using ABCinema_WPF.Logging;
using ABCinema_WPF.Models;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace ABCinema_WPF.Data.Repositories;

public static class CinemaUserRepository
{
    public static async Task<List<CinemaUser>> GetAll()
    {
        var users = new List<CinemaUser>();
        await using var conn = DbConnectionFactory.CreateConnection();
        await conn.OpenAsync();
        await using var cmd = new NpgsqlCommand("SELECT * FROM cinema_user", conn);
        await using var reader = await cmd.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            users.Add(new CinemaUser(
                reader.GetInt32(0),
                reader.GetString(1),
                reader.GetString(2),
                reader.GetString(3),
                reader.GetBoolean(4),
                reader.GetDateTime(5)
            ));
        }
        return users;
    }

    public static async Task Create(CinemaUser user)
    {
        await using var conn = DbConnectionFactory.CreateConnection();
        await conn.OpenAsync();
        await using var cmd = new NpgsqlCommand(@"
            INSERT INTO cinema_user (username, email, password_hash, is_admin)
            VALUES (@username, @email, @password_hash, @is_admin)", conn);
        
        cmd.Parameters.AddWithValue("username", user.Username);
        cmd.Parameters.AddWithValue("email", user.Email);
        cmd.Parameters.AddWithValue("password_hash", user.PasswordHash);
        cmd.Parameters.AddWithValue("is_admin", user.IsAdmin);
        await cmd.ExecuteNonQueryAsync();
        AppLogger.LogInfo($"Регистрация {user.Username} : {user.Email} успешна завершена");
    }
    
    public static async Task<string> GetUserHash(CinemaUser user)
    {
        await using var conn = DbConnectionFactory.CreateConnection();
        await conn.OpenAsync();
        await using var cmd = new NpgsqlCommand(@"
        SELECT password_hash FROM cinema_user WHERE email = @email", conn);
        
        cmd.Parameters.AddWithValue("email", user.Email);
        await using var reader = await cmd.ExecuteReaderAsync();
        await reader.ReadAsync();
        return reader.GetString(0);
    }
    
    public static async Task<string> IsAdmin(CinemaUser user)
    {
        await using var conn = DbConnectionFactory.CreateConnection();
        await conn.OpenAsync();
        await using var cmd = new NpgsqlCommand(@"
        SELECT is_admin FROM cinema_user WHERE email = @email", conn);
        
        cmd.Parameters.AddWithValue("email", user.Email);
        await using var reader = await cmd.ExecuteReaderAsync();
        await reader.ReadAsync();
        var result = reader.GetString(0);
        return result;
    }
    
    public static async Task<bool> IsUserExist(CinemaUser user)
    {
        await using var conn = DbConnectionFactory.CreateConnection();
        await conn.OpenAsync();
        await using var cmd = new NpgsqlCommand(@"
        SELECT EXISTS(
        SELECT 1 FROM cinema_user WHERE email = @email
        );", conn);
        
        cmd.Parameters.AddWithValue("email", user.Email);
        var result = (bool?)await cmd.ExecuteScalarAsync();
        AppLogger.LogInfo($"Существование пользователя {user.Username} : {user.Email} - {result}");
        return result ?? false;
    }
    
    public static async Task<CinemaUser?> Get(string email)
    {
        CinemaUser user = null;
        await using var conn = DbConnectionFactory.CreateConnection();
        await conn.OpenAsync();
        await using var cmd = new NpgsqlCommand("SELECT * FROM cinema_user", conn);
        await using var reader = await cmd.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            user = new CinemaUser(
                reader.GetInt32(0),
                reader.GetString(1),
                reader.GetString(2),
                reader.GetString(3),
                reader.GetBoolean(4),
                reader.GetDateTime(5)
            );
        }
        return user;
    }
}
