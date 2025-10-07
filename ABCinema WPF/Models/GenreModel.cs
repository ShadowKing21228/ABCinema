using ABCinema_WPF.Data.Repositories;

namespace ABCinema_WPF.Models;

public static class GenreModel
{
    public static async Task<List<Genre>> GetAllGenres() 
        => await GenreRepository.GetAll();
    
    public static async Task CreateGenre(Genre genre) 
        => await GenreRepository.Create(genre);

    public static async Task UpdateGenre(Genre genre)
        => await GenreRepository.Update(genre);

    
}