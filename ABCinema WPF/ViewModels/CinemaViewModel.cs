using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using ABCinema_WPF.Controls;
using ABCinema_WPF.Models;
using ABCinema_WPF.Utils;
using MahApps.Metro.Controls.Dialogs;

namespace ABCinema_WPF.ViewModels;

public class CinemaViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;
    
    public ICommand ShowAddCinemaDialogCommand { get; set;}
    
    public ICommand CloseCinemaDialogCommand { get; set; }
    
    public ICommand AddCinemaCommand { get; set; }
    
    public ICommand ShowUpdateCinemaDialogCommand { get; }
    
    public ICommand CloseUpdateCinemaDialogCommand { get; }
    
    public ICommand UpdateCinemaCommand { get; }
    
    public ObservableCollection<MovieItem> MovieItems { get; set; }
    
    public MovieItem? SelectedMovie
    {
        get => _selectedMovie;
        set
        {
            _selectedMovie = value;
            OnPropertyChanged(nameof(SelectedMovie));
        }
    }
    private MovieItem? _selectedMovie;
    
    public ObservableCollection<Genre> AllGenres { get; set; }
    public ObservableCollection<Genre> SelectedGenres { get; set; } = [];

    private bool _isGenreFlyoutOpen;
    public bool IsGenreFlyoutOpen
    {
        get => _isGenreFlyoutOpen;
        set
        {
            _isGenreFlyoutOpen = value;
            OnPropertyChanged(nameof(IsGenreFlyoutOpen));
        }
    }


    public ICommand ToggleGenreFlyoutCommand { get; }
    
    private IDialogCoordinator DialogCoordinator { get; set; }

    private readonly CinemaDialog _dialog;
    
    private readonly CinemaUpdateDialog _updateDialog;

    private CinemaViewModel(ObservableCollection<MovieItem> movieItems, ObservableCollection<Genre> genreItems, IDialogCoordinator dialogCoordinator)
    {
        MovieItems = movieItems;
        DialogCoordinator = dialogCoordinator;
        AllGenres = genreItems;
        _dialog = new CinemaDialog { DataContext = this };
        _updateDialog = new CinemaUpdateDialog { DataContext = this };
        IsGenreFlyoutOpen = false;
        ToggleGenreFlyoutCommand = new RelayCommand(async _ => {
            IsGenreFlyoutOpen = !IsGenreFlyoutOpen;
            await Task.CompletedTask;
        });
        
        ShowUpdateCinemaDialogCommand = new RelayCommand(ShowUpdateDialog);
        CloseUpdateCinemaDialogCommand = new RelayCommand(HideUpdateDialog);
        UpdateCinemaCommand = new RelayCommand(UpdateCinema);
        AddCinemaCommand = new RelayCommand(AddCinema);
        ShowAddCinemaDialogCommand = new RelayCommand(ShowDialog);
        CloseCinemaDialogCommand = new RelayCommand(HideCinemaDialog);
    }

    public static async Task<CinemaViewModel> CreateAsync(IDialogCoordinator dialogCoordinator)
        => new(new ObservableCollection<MovieItem>(await CinemaModel.GetAllMovie()), new ObservableCollection<Genre>(await GenreModel.GetAllGenres()), dialogCoordinator);
    

    private async Task ShowDialog(object? sender) => await DialogCoordinator.ShowMetroDialogAsync(this, _dialog);
    
    private async Task HideCinemaDialog(object? sender) => await DialogCoordinator.HideMetroDialogAsync(this, _dialog);

    private async Task AddCinema(object? sender)
    {
        if (string.IsNullOrWhiteSpace(_dialog.CinemaNameBox.Text) ||
            string.IsNullOrWhiteSpace(_dialog.DescriptionBox.Text) ||
            string.IsNullOrWhiteSpace(_dialog.DurationMinutesBox.Text) ||
            string.IsNullOrWhiteSpace(_dialog.RatingBox.Text) ||
            string.IsNullOrWhiteSpace(_dialog.GetSelectedImagePath()))
            return;

        if (!int.TryParse(_dialog.DurationMinutesBox.Text, out var durationMinutes) ||
            !float.TryParse(_dialog.RatingBox.Text, out var rating))
            return;

        var movie = new Movie(
            0, // пусть автоинкремент
            _dialog.CinemaNameBox.Text,
            _dialog.DescriptionBox.Text,
            durationMinutes,
            rating,
            _dialog.GetSelectedImagePath()
        );

        var movieId = await CinemaModel.AddMovieAndGetId(movie);

        foreach (var genre in SelectedGenres) {
            await CinemaModel.LinkGenreToMovie(movieId, genre.Id);
        }

        await HideCinemaDialog(this);
    }

    
    private async Task ShowUpdateDialog(object? parameter)
    {
        if (SelectedMovie == null) return;

        _updateDialog.CinemaNameBox.Text = SelectedMovie.Title;
        _updateDialog.DescriptionBox.Text = SelectedMovie.Description;
        _updateDialog.DurationMinutesBox.Text = SelectedMovie.DurationMinutes.ToString();
        _updateDialog.RatingBox.Text = SelectedMovie.Rating.ToString("0.0");
        _updateDialog.PosterPreview.Source = SelectedMovie.Poster;

        await DialogCoordinator.ShowMetroDialogAsync(this, _updateDialog);
    }

    private async Task HideUpdateDialog(object? parameter)
        => await DialogCoordinator.HideMetroDialogAsync(this, _updateDialog);

    
    private async Task UpdateCinema(object? parameter)
    {
        if (SelectedMovie == null) return;

        var title = _updateDialog.CinemaNameBox.Text;
        var description = _updateDialog.DescriptionBox.Text;
        var poster = _updateDialog.GetSelectedImagePath();

        if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(description)
                                             || !int.TryParse(_updateDialog.DurationMinutesBox.Text, out var duration)
                                             || !float.TryParse(_updateDialog.RatingBox.Text, out var rating))
            return;
        

        await CinemaModel.UpdateMovie(new Movie(SelectedMovie.Id, title, description, duration, rating, poster));
        
        await HideUpdateDialog(parameter);
    }
    
    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MovieItems)));
    }
}