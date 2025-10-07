using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using ABCinema_WPF.Controls;
using ABCinema_WPF.Models;
using ABCinema_WPF.Utils;
using MahApps.Metro.Controls.Dialogs;

namespace ABCinema_WPF.ViewModels;

public class GenreViewModel : INotifyPropertyChanged
{
    public ICommand ShowGenreDialogCommand { get; set; }
    
    public ICommand EditGenreCommand { get; set; }
    
    public event PropertyChangedEventHandler? PropertyChanged;
    
    public ObservableCollection<Genre> Genres { get; set; }
    
    private Genre _selectedGenre;
    public Genre SelectedGenre
    {
        get => _selectedGenre;
        set
        {
            _selectedGenre = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedGenre)));
        }
    }
    
    private readonly IDialogCoordinator _dialogCoordinator;

    private GenreViewModel(ObservableCollection<Genre> halls, IDialogCoordinator dialogCoordinator)
    {
        _dialogCoordinator = dialogCoordinator;
        Genres = halls;

        ShowGenreDialogCommand = new RelayCommand(ShowDialog);
        EditGenreCommand = new RelayCommand(EditGenre);

    }

    private async Task ShowDialog(object? parameter)
    {
        var var = await _dialogCoordinator.ShowInputAsync(this, "Добавление жанра", "Напишите название нового жанра");
        
        if (string.IsNullOrEmpty(var)) return;

        await GenreModel.CreateGenre(new Genre(1, var));
    }
    
    private async Task EditGenre(object? parameter)
    {
        var input = await _dialogCoordinator.ShowInputAsync(this, "Редактирование жанра", $"Новое название для жанра \"{SelectedGenre.Name}\"");

        if (string.IsNullOrWhiteSpace(input)) return;

        var updatedGenre = SelectedGenre with { Name = input };

        await GenreModel.UpdateGenre(updatedGenre);

        // Обновление в UI
        var index = Genres.IndexOf(SelectedGenre);
        if (index >= 0)
            Genres[index] = updatedGenre;

        SelectedGenre = updatedGenre;
    }

    
    public static async Task<GenreViewModel> GenreViewModelFactory(IDialogCoordinator dialogCoordinator) =>
        new(new ObservableCollection<Genre>(await GenreModel.GetAllGenres()), dialogCoordinator);
}