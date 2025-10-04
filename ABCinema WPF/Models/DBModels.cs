using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows.Media;

namespace ABCinema_WPF.Models;

public record class CinemaUser(
    int? Id,
    string Username,
    string Email,
    string PasswordHash,
    bool IsAdmin,
    DateTime CreatedAt
);

public record class Genre(
    int Id,
    string Name
);

public record class Movie(
    int Id,
    string Title,
    string? Description,
    int DurationMinutes,
    float Rating,
    string Poster
);

public record class Hall(
    int Id,
    string Name,
    int Rows,
    int SeatsPerRow
);

public record class Seat(
    int Id,
    int HallId,
    int RowNumber,
    int SeatNumber
);

public record class Session(
    int Id,
    int MovieId,
    int HallId,
    DateTime StartTime,
    DateTime EndTime,
    decimal BasePrice,
    bool IsExpired
);

public record class SeatPrice(
    int Id,
    int SessionId,
    int SeatId,
    decimal Price
);

public record class SeatReservation(
    int Id,
    int SessionId,
    int SeatId,
    int? UserId,
    DateTime ReservedAt
);

public record LoginModel(
    string Email,
    string Password
    );

public record AfishaItem(
    int MovieId,
    string MovieTitle,
    ImageSource Poster,
    float Rating,
    ObservableCollection<Session> Sessions
);

public record MovieItem(
    int Id,
    string Title,
    string? Description,
    int DurationMinutes,
    float Rating,
    ImageSource Poster
);

public record HallItem(
    int Id,
    string Name,
    int Rows,
    int SeatsPerRow,
    int Capacity
    );