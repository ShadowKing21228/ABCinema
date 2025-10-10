using ABCinema_WPF.Logging;
using ABCinema_WPF.Models;
using Npgsql;

namespace ABCinema_WPF.Data.Repositories;

public static class MovieRepository
{
    public static async Task<List<Movie>> GetAll()
    {
        var movies = new List<Movie>();
        await using var conn = DbConnectionFactory.CreateConnection();
        await conn.OpenAsync();
        await using var cmd = new NpgsqlCommand("SELECT * FROM movie", conn);
        await using var reader = await cmd.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            movies.Add(new Movie(
                reader.GetInt32(0),
                reader.GetString(1),
                reader.IsDBNull(2) ? null : reader.GetString(2),
                reader.GetInt32(3),
                reader.GetFloat(4),
                reader.IsDBNull(5) ? null : reader.GetString(5)
                ));
        }
        return movies;
    }
    
    public static async Task<Movie> GetMovie(int id)
    {
        Movie movie = null;
        await using var conn = DbConnectionFactory.CreateConnection();
        conn.Open();
        await using var cmd = new NpgsqlCommand("SELECT * FROM movie WHERE id = @id", conn);
        cmd.Parameters.AddWithValue("@id", id);
        await using var reader = await cmd.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            movie = new Movie(
                reader.GetInt32(0),
                reader.GetString(1),
                reader.IsDBNull(2) ? null : reader.GetString(2),
                reader.GetInt32(3),
                reader.GetFloat(4),
                reader.IsDBNull(5) ? null : reader.GetString(5)
                );
        }
        AppLogger.LogInfo($"Запрос фильма {movie.Id} : {movie.Title} успешно завершён");
        return movie;
    }
    
    public static async Task AddMovie(Movie movie)
    {
        await using var conn = DbConnectionFactory.CreateConnection();
        await conn.OpenAsync();
        await using var cmd = new NpgsqlCommand("INSERT INTO movie(title, description, duration_minutes, rating, poster_url) VALUES (@title, @description, @durationMinutes, @rating, @posterUrl)", conn);
        cmd.Parameters.AddWithValue("@title", movie.Title);
        cmd.Parameters.AddWithValue("@description", movie.Description);
        cmd.Parameters.AddWithValue("@durationMinutes", movie.DurationMinutes);
        cmd.Parameters.AddWithValue("@rating", movie.Rating);
        cmd.Parameters.AddWithValue("@posterUrl", movie.Poster);
        
        await cmd.ExecuteNonQueryAsync();
        
        AppLogger.LogInfo($"Добавление фильма {movie.Id} : {movie.Title} успешно завершено");
    }
    
    public static async Task Update(Movie movie)
    {
        await using var conn = DbConnectionFactory.CreateConnection();
        await conn.OpenAsync();

        await using var cmd = new NpgsqlCommand("""
                                                    UPDATE movie SET title = @title, description = @desc, duration_minutes = @dur, rating = @rating, poster_url = @poster
                                                    WHERE id = @id
                                                """, conn);

        cmd.Parameters.AddWithValue("title", movie.Title);
        cmd.Parameters.AddWithValue("desc", movie.Description);
        cmd.Parameters.AddWithValue("dur", movie.DurationMinutes);
        cmd.Parameters.AddWithValue("rating", movie.Rating);
        cmd.Parameters.AddWithValue("poster", movie.Poster);
        cmd.Parameters.AddWithValue("id", movie.Id);

        AppLogger.LogInfo($"Фильм {movie.Id} обновлён: {movie.Title}");
        await cmd.ExecuteNonQueryAsync();
    }

    public static async Task<int> AddMovieAndGetId(Movie movie)
    {
        await using var conn = DbConnectionFactory.CreateConnection();
        await conn.OpenAsync();

        await using var cmd = new NpgsqlCommand("""
                                                    INSERT INTO movie (title, description, duration_minutes, rating, poster_url)
                                                    VALUES (@title, @desc, @dur, @rating, @poster)
                                                    RETURNING id
                                                """, conn);

        cmd.Parameters.AddWithValue("title", movie.Title);
        cmd.Parameters.AddWithValue("desc", movie.Description);
        cmd.Parameters.AddWithValue("dur", movie.DurationMinutes);
        cmd.Parameters.AddWithValue("rating", movie.Rating);
        cmd.Parameters.AddWithValue("poster", movie.Poster);

        var id = (int)await cmd.ExecuteScalarAsync();
        
        AppLogger.LogInfo($"Фильм {movie.Title} добавлен с ID {id}");
        return id;
    }
    
    public static async Task LinkGenreToMovie(int movieId, int genreId)
    {
        await using var conn = DbConnectionFactory.CreateConnection();
        await conn.OpenAsync();

        await using var cmd = new NpgsqlCommand("""
                                                    INSERT INTO movie_genre (movie_id, genre_id)
                                                    VALUES (@movie, @genre)
                                                """, conn);

        cmd.Parameters.AddWithValue("movie", movieId);
        cmd.Parameters.AddWithValue("genre", genreId);

        await cmd.ExecuteNonQueryAsync();
        AppLogger.LogInfo($"Жанр {genreId} привязан к фильму {movieId}");
    }
}
