using ABCinema_WPF.Data.Repositories;
using ABCinema_WPF.Logging;
using ABCinema_WPF.Security;

namespace ABCinema_WPF.Models;

public static class AuthModel
{
    public static async Task<bool> Register(string username, string email, string password)
    {
        var hash = PasswordUtils.HashPassword(password);
        var cinemaUser = new CinemaUser(null, username, email, hash, false, DateTime.Now);
        AppLogger.LogInfo($"Начало регистрации пользователя {username} : {email}");

        if (await CinemaUserRepository.IsUserExist(cinemaUser)) {
            AppLogger.LogWarning($"Пользователь {email} уже существует");
            return false;
        }
        
        await CinemaUserRepository.Create(cinemaUser);
        return true;
    }
    
    public static async Task<bool> Login(string username, string email, string password)
    {
        var var = await CinemaUserRepository.GetUserHash(new CinemaUser(null, email, username, password, false, DateTime.Now));
        return PasswordUtils.VerifyPassword(var, password);
    }
}