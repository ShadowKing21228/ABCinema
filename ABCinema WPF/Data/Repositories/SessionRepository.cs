using ABCinema_WPF.Logging;
using ABCinema_WPF.Models;
using Npgsql;

namespace ABCinema_WPF.Data.Repositories;

public static class SessionRepository
{
    public static async Task<List<Session>> GetAllByDate(DateTime date)
    {
        var movies = new List<Session>();
        await using var conn = DbConnectionFactory.CreateConnection();
        conn.Open();
        await using var cmd = new NpgsqlCommand("SELECT * FROM session WHERE start_time::date = @date", conn);
        cmd.Parameters.AddWithValue("@date", date.Date);
        await using var reader = await cmd.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            var dateTime = reader.GetDateTime(3);
            movies.Add(new Session(
                reader.GetInt32(0),
                reader.GetInt32(1),
                reader.GetInt32(2),
                dateTime,
                reader.GetDateTime(4),
                reader.GetDecimal(5),
                DateTime.Now > dateTime
                ));
        }
        AppLogger.LogInfo($"Запрос всех сеансов в {date.Date} успешно завершён");
        return movies;
    }
    
    public static async Task Add(Session session)
    {
        await using var conn = DbConnectionFactory.CreateConnection();
        await conn.OpenAsync();
        await using var cmd = new NpgsqlCommand("INSERT INTO session(movie_id, hall_id, start_time, end_time, base_price) VALUES (@movieId, @hallId, @startTime, @endTime, @basePrice)", conn);
        cmd.Parameters.AddWithValue("@movieId", session.MovieId);
        cmd.Parameters.AddWithValue("@hallId", session.HallId);
        cmd.Parameters.AddWithValue("@startTime", session.StartTime);
        cmd.Parameters.AddWithValue("@endTime", session.EndTime);
        cmd.Parameters.AddWithValue("@basePrice", session.BasePrice);
        await cmd.ExecuteNonQueryAsync();
        AppLogger.LogInfo("Добавление сессии успешно завершено");
    }
    
    public static async Task Update(Session session)
    {
        await using var conn = DbConnectionFactory.CreateConnection();
        await conn.OpenAsync();

        await using var cmd = new NpgsqlCommand("""
                                                    UPDATE session
                                                    SET movie_id = @movie, hall_id = @hall, start_time = @start, end_time = @end, base_price = @price
                                                    WHERE id = @id
                                                """, conn);

        cmd.Parameters.AddWithValue("id", session.Id);
        cmd.Parameters.AddWithValue("movie", session.MovieId);
        cmd.Parameters.AddWithValue("hall", session.HallId);
        cmd.Parameters.AddWithValue("start", session.StartTime);
        cmd.Parameters.AddWithValue("end", session.EndTime);
        cmd.Parameters.AddWithValue("price", session.BasePrice);

        AppLogger.LogInfo($"Сеанс {session.Id} обновлён: {session.MovieId} в зале {session.HallId}");
        await cmd.ExecuteNonQueryAsync();
    }

}