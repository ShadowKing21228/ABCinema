using ABCinema_WPF.Data.Repositories;

namespace ABCinema_WPF.Models;

public static class BuyTicketModel
{
    public static async Task<Hall> GetHall(int id) 
        => await HallRepository.Get(id);

    public static async Task AddReservation(Session session, SeatItem seatItem) 
        => await SeatReservationRepository.AddReservation(session, seatItem);
    
    public static async Task<List<ReservedSeat>> GetReservationSeats(Session session)
        => await SeatReservationRepository.GetAllInSession(session);
    
}