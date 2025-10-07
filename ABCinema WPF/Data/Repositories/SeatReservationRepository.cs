using ABCinema_WPF.Logging;
using ABCinema_WPF.Models;
using Npgsql;

namespace ABCinema_WPF.Data.Repositories;

public static class SeatReservationRepository
{
    public static async Task AddReservation(Session session, SeatItem seatItem)
    {
        await using var conn = DbConnectionFactory.CreateConnection();
        await conn.OpenAsync();
        await using var cmd = new NpgsqlCommand("INSERT INTO seat_reservation(session_id, row_number, seat_number) VALUES (@sessionId, @rowNumber, @seatNumber)", conn);
        cmd.Parameters.AddWithValue("@sessionId", session.Id);
        cmd.Parameters.AddWithValue("@rowNumber", seatItem.RowNumber);
        cmd.Parameters.AddWithValue("@seatNumber", seatItem.SeatNumber);
        
        await cmd.ExecuteNonQueryAsync();
        
        AppLogger.LogInfo($"Покупка на сеансе {session.Id} места {seatItem.RowNumber} x {seatItem.SeatNumber} успешна завершена");
    }
    
    public static async Task<List<ReservedSeat>> GetAllInSession(Session session)
    {
        List<ReservedSeat> reservedSeat = [];
        await using var conn = DbConnectionFactory.CreateConnection();
        await conn.OpenAsync();
        await using var cmd = new NpgsqlCommand("SELECT * FROM seat_reservation WHERE session_id = @id", conn);
        
        cmd.Parameters.AddWithValue("id", session.Id);
        
        var reader = await cmd.ExecuteReaderAsync();
        while (await reader.ReadAsync()) {
            reservedSeat.Add(new ReservedSeat(
                reader.GetInt32(0),
                reader.GetInt32(1),
                reader.GetInt32(2),
                reader.GetInt32(3)
            ));
        }
        
        AppLogger.LogInfo($"Зарезервированные места в сессии \"{session.Id}\" успешно получены!");
        
        return reservedSeat;
    }
}