using ABCinema_WPF.Data.Repositories;
using ABCinema_WPF.Models;

namespace ABCinema_WPF.Data;

public static class UserSession
{
    
    private static CinemaUser? _user;

    public static async Task RecordUserData(string email) {
        _user = await CinemaUserRepository.Get(email);
    }

    public static void ClearUserData() => _user = null;
    
    public static CinemaUser? GetUser() => _user;
}
