using ABCinema_WPF.Models;
using Npgsql;

namespace ABCinema_WPF.Data.Repositories;

public static class GenreRepository
{
    public static List<Genre> GetAll()
    {
        var genres = new List<Genre>();
        using var conn = DbConnectionFactory.CreateConnection();
        conn.Open();
        using var cmd = new NpgsqlCommand("SELECT * FROM genre", conn);
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            genres.Add(new Genre(
                reader.GetInt32(0),
                reader.GetString(1)
            ));
        }
        return genres;
    }

    public static void Create(string name)
    {
        using var conn = DbConnectionFactory.CreateConnection();
        conn.Open();
        using var cmd = new NpgsqlCommand("INSERT INTO genre (name) VALUES (@name)", conn);
        cmd.Parameters.AddWithValue("name", name);
        cmd.ExecuteNonQuery();
    }
}
