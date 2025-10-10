using System.Collections.ObjectModel;
using ABCinema_WPF.Data.Repositories;
using ABCinema_WPF.Utils;

namespace ABCinema_WPF.Models;

public static class AfishaModel
{
    public static async Task<ObservableCollection<AfishaItem>> GetAfishaItemsAsync(DateTime date)
    {
        var sessions = await SessionRepository.GetAllByDate(date);
        var items = new ObservableCollection<AfishaItem>();
        
        var movieList = new List<int>();
        
        foreach (var session in sessions.Where(session => !movieList.Contains(session.MovieId)))
        {
            var movie = await MovieRepository.GetMovie(session.MovieId);
            
            var sessionList = new ObservableCollection<Session>(sessions.Where(session1 => session1.MovieId == session.MovieId));
            movieList.Add(session.MovieId);
            
            items.Add(new AfishaItem(
                movie.Id,
                movie.Title,
                ImageConverter.UriToImageSource(movie.Poster),
                movie.Rating,
                sessionList
            ));
        }
        
        return items;
    }

    public static async Task AddSession(Session session)
    {
        await SessionRepository.Add(session);
    }
    
    public static async Task UpdateSessionAsync(Session session)
        => await SessionRepository.Update(session);

}