using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
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
    
    public ObservableCollection<MovieItem> MovieItems { get; set; }

    private IDialogCoordinator DialogCoordinator { get; set; }

    private CinemaDialog _dialog;

    private CinemaViewModel(ObservableCollection<MovieItem> movieItems, IDialogCoordinator dialogCoordinator)
    {
        MovieItems = movieItems;
        DialogCoordinator = dialogCoordinator;
        _dialog = new CinemaDialog { DataContext = this };
        AddCinemaCommand = new RelayCommand(AddCinema);
        ShowAddCinemaDialogCommand = new RelayCommand(ShowDialog);
        CloseCinemaDialogCommand = new RelayCommand(HideCinemaDialog);
    }

    public static async Task<CinemaViewModel> CreateAsync(IDialogCoordinator dialogCoordinator) => new(new ObservableCollection<MovieItem>(await CinemaModel.GetAllMovie()), dialogCoordinator);
    
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
            1,
            _dialog.CinemaNameBox.Text,
            _dialog.DescriptionBox.Text,
            durationMinutes,
            rating,
            _dialog.GetSelectedImagePath()
        );
        
        await CinemaModel.AddMovie(movie);
        
        await HideCinemaDialog(this);
    }
    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MovieItems)));
    }
}