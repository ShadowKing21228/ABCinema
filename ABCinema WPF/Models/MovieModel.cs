using System.IO;
using ABCinema_WPF.Data.Repositories;
using ABCinema_WPF.Utils;

namespace ABCinema_WPF.Models;

public static class MovieModel
{
    public static async Task<List<MovieItem>> GetAllMovies()
    {
        var var = await MovieRepository.GetAll();
        return var.Select(item => 
            new MovieItem(item.Id, item.Title, item.Description, item.DurationMinutes, item.Rating, ImageConverter.UriToImageSource(item.Poster))).ToList();
    }
    
    
    public static async Task UpdateMovie(Movie movie)
        => await MovieRepository.Update(movie);

    
    public static async Task<int> AddMovieAndGetId(Movie movie) 
    =>  await MovieRepository.AddMovieAndGetId(movie);

    public static async Task LinkGenreToMovie(int movieId, int genreId)
        => await MovieRepository.LinkGenreToMovie(movieId, genreId);
}