using ABCinema_WPF.Models;
using Npgsql;

namespace ABCinema_WPF.Data.Repositories;

public class HallRepository
{
    public static async Task AddHall(Hall hall)
    {
        await using var conn = DbConnectionFactory.CreateConnection();
        await conn.OpenAsync();
        await using var cmd = new NpgsqlCommand("INSERT INTO hall(name, rows, seats_per_row) VALUES (@name, @rows, @seatsPerRow)", conn);
        cmd.Parameters.AddWithValue("name", hall.Name);
        cmd.Parameters.AddWithValue("rows", hall.Rows);
        cmd.Parameters.AddWithValue("seatsPerRow", hall.SeatsPerRow);
        
        await cmd.ExecuteNonQueryAsync();
    }
    
    public static async Task<List<Hall>> GetAll()
    {
        List<Hall> halls = [];
        await using var conn = DbConnectionFactory.CreateConnection();
        await conn.OpenAsync();
        await using var cmd = new NpgsqlCommand("SELECT * FROM hall", conn);
        
        var reader = await cmd.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            halls.Add(new Hall(
                reader.GetInt32(0),
                reader.GetString(1),
                reader.GetInt32(2),
                reader.GetInt32(3)
                ));
        }

        return halls;
    }
}