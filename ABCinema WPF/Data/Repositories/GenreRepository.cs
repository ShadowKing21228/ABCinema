using ABCinema_WPF.Logging;
using ABCinema_WPF.Models;
using Npgsql;

namespace ABCinema_WPF.Data.Repositories;

public static class GenreRepository
{
    public static async Task<List<Genre>> GetAll()
    {
        var genres = new List<Genre>();
        await using var conn = DbConnectionFactory.CreateConnection();
        await conn.OpenAsync();
        await using var cmd = new NpgsqlCommand("SELECT * FROM genre", conn);
        await using var reader = await cmd.ExecuteReaderAsync();
        
        while (await reader.ReadAsync())
        {
            genres.Add(new Genre(
                reader.GetInt32(0),
                reader.GetString(1)
            ));
        }
        return genres;
    }

    public static async Task Create(Genre genre)
    {
        await using var conn = DbConnectionFactory.CreateConnection();
        await conn.OpenAsync();
        
        await using var cmd = new NpgsqlCommand("INSERT INTO genre (name) VALUES (@name)", conn);
        cmd.Parameters.AddWithValue("name", genre.Name);
        
        AppLogger.LogInfo($"Жанр {genre.Name} успешно добавлен!");
        await cmd.ExecuteNonQueryAsync();
    }
    
    public static async Task Update(Genre genre)
    {
        await using var conn = DbConnectionFactory.CreateConnection();
        await conn.OpenAsync();

        await using var cmd = new NpgsqlCommand("UPDATE genre SET name = @name WHERE id = @id", conn);
        cmd.Parameters.AddWithValue("name", genre.Name);
        cmd.Parameters.AddWithValue("id", genre.Id);

        AppLogger.LogInfo($"Жанр {genre.Id} обновлён на \"{genre.Name}\"");
        await cmd.ExecuteNonQueryAsync();
    }

}
