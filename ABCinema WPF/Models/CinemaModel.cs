using System.IO;
using ABCinema_WPF.Data.Repositories;
using ABCinema_WPF.Utils;

namespace ABCinema_WPF.Models;

public static class CinemaModel
{
    public static async Task AddMovie(Movie movie)
    {
        var targetPath = Path.Combine("Resources/Images/Posters", Path.GetFileName(movie.Poster));
        File.Copy(movie.Poster, targetPath, overwrite: true);
        await MovieRepository.AddMovie(movie with { Poster = targetPath });
    }


    public static async Task<List<MovieItem>> GetAllMovie()
    {
        var var = await MovieRepository.GetAll();
        return var.Select(item => 
            new MovieItem(item.Id, item.Title, item.Description, item.DurationMinutes, item.Rating, ImageConverter.UriToImageSource(item.Poster))).ToList();
    }
}