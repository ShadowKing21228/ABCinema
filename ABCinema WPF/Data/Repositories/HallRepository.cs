using ABCinema_WPF.Logging;
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
    
    public static async Task<Hall> Get(int id)
    {
        Hall hall = null;
        await using var conn = DbConnectionFactory.CreateConnection();
        await conn.OpenAsync();
        await using var cmd = new NpgsqlCommand("SELECT * FROM hall WHERE id = @id", conn);
        
        cmd.Parameters.AddWithValue("id", id);
        
        var reader = await cmd.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            hall = new Hall(
                reader.GetInt32(0),
                reader.GetString(1),
                reader.GetInt32(2),
                reader.GetInt32(3)
            );
        }

        if (hall != null) AppLogger.LogInfo($"Зал \"{id}\" / {hall.Name} : {hall.Rows} x {hall.SeatsPerRow} успешно получен!");
        
        return hall;
    }
    
    public static async Task Update(Hall hall)
    {
        await using var conn = DbConnectionFactory.CreateConnection();
        await conn.OpenAsync();

        await using var cmd = new NpgsqlCommand("UPDATE hall SET name = @name, rows = @rows, seats_per_row = @seats WHERE id = @id", conn);
        cmd.Parameters.AddWithValue("name", hall.Name);
        cmd.Parameters.AddWithValue("rows", hall.Rows);
        cmd.Parameters.AddWithValue("seats", hall.SeatsPerRow);
        cmd.Parameters.AddWithValue("id", hall.Id);

        AppLogger.LogInfo($"Зал {hall.Id} обновлён: {hall.Name}, {hall.Rows}×{hall.SeatsPerRow}");
        await cmd.ExecuteNonQueryAsync();
    }

}