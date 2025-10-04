using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using ABCinema_WPF.Controls;
using ABCinema_WPF.Models;
using ABCinema_WPF.Utils;
using MahApps.Metro.Controls.Dialogs;

namespace ABCinema_WPF.ViewModels;

public class AfishaViewModel : INotifyPropertyChanged
{
    
    public ICommand TodayButtonClick { get; set; }
    
    public ICommand TomorrowButtonClick { get; set; }
    
    public ICommand DayAfterTomorrowButtonClick { get; set; }
    
    public ICommand ShowSessionDialogCommand { get; set; }
    
    public ICommand AddSessionCommand { get; set; }
    
    public ICommand CloseSessionDialogCommand { get; set; }
    
    private DateTime _selectedDate;
    
    public DateTime SelectedDate
    {
        get => _selectedDate;
        
        set {
            if (_selectedDate == value) return;
            _selectedDate = value;
            _ = LoadSessionsAsync();
        }
    }
    
    public ObservableCollection<AfishaItem> AfishaItems { get; set; }

    private IDialogCoordinator DialogCoordinator { get; set; }

    private SessionDialog _sessionDialog;
    
    public event PropertyChangedEventHandler? PropertyChanged;
    
    //public ICommand BuyCommand = new RelayCommand<Movie>(BuyTicket);

    private AfishaViewModel(ObservableCollection<AfishaItem> afishaItems, IDialogCoordinator coordinator)
    {
        AfishaItems = afishaItems;
        DialogCoordinator = coordinator;
        _sessionDialog = new SessionDialog { DataContext = this };
        TodayButtonClick = new RelayCommand(SelectTodayButtonClick);
        TomorrowButtonClick = new RelayCommand(SelectTomorrowButtonClick);
        DayAfterTomorrowButtonClick = new RelayCommand(SelectDayAfterTomorrowButtonClick);
        ShowSessionDialogCommand = new RelayCommand(ShowAddSessionDialog);
        AddSessionCommand = new RelayCommand(AddSession);
        CloseSessionDialogCommand = new RelayCommand(CloseAddSessionDialog);
    }
    
    public static async Task<AfishaViewModel> AfishaViewModelFactory(DateTime dateTime, IDialogCoordinator coordinator) 
    
        => new(await AfishaModel.GetAfishaItemsAsync(dateTime), coordinator);
    

    private async Task SelectTodayButtonClick(object? parameter)
    {
        SelectedDate = DateTime.Today;
        await LoadSessionsAsync();
    }
    
    private async Task SelectTomorrowButtonClick(object? parameter)
    {
        SelectedDate = DateTime.Today.AddDays(1);
        await LoadSessionsAsync();
    }
    
    private async Task SelectDayAfterTomorrowButtonClick(object? parameter)
    {
        SelectedDate = DateTime.Today.AddDays(2);
        await LoadSessionsAsync();
    }
    private async Task LoadSessionsAsync()
    {
        var afishaItems = await AfishaModel.GetAfishaItemsAsync(_selectedDate);
        AfishaItems = new ObservableCollection<AfishaItem>(afishaItems);
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AfishaItems)));
    }

    private async Task ShowAddSessionDialog(object? parameter)
    {
        var settings = new MetroDialogSettings
        {
            AnimateShow = true
        };

        await DialogCoordinator.ShowMetroDialogAsync(this, _sessionDialog, settings);
    }

    private async Task AddSession(object? parameter)
    {
        if (string.IsNullOrWhiteSpace(_sessionDialog.CinemaIdBox.Text) ||
            string.IsNullOrWhiteSpace(_sessionDialog.HallIdBox.Text) ||
            string.IsNullOrWhiteSpace(_sessionDialog.PriceBox.Text) ||
            !_sessionDialog.StartDateTime.SelectedDateTime.HasValue ||
            !_sessionDialog.EndDateTime.SelectedDateTime.HasValue)
        {
            return;
        }
        
        if (!int.TryParse(_sessionDialog.CinemaIdBox.Text, out int movieId) ||
            !int.TryParse(_sessionDialog.HallIdBox.Text, out int hallId) ||
            !decimal.TryParse(_sessionDialog.PriceBox.Text, out decimal price))
        {
            return;
        }

        var startTime = _sessionDialog.StartDateTime.SelectedDateTime.Value;
        var endTime = _sessionDialog.EndDateTime.SelectedDateTime.Value;
        
        var session = new Session(
            1,
            movieId,
            hallId,
            startTime,
            endTime,
            price,
            startTime > DateTime.Now
        );
        
        await AfishaModel.AddSession(session);
        
        await DialogCoordinator.HideMetroDialogAsync(this, _sessionDialog);
    }
    
    private async Task CloseAddSessionDialog(object? parameter) => await DialogCoordinator.HideMetroDialogAsync(this, _sessionDialog);
    

    //protected async Task OnPropertyChanged(PropertyChangedEventArgs e)
    //{
    //    await LoadSessionsAsync();
    //    PropertyChanged?.Invoke(this, e);
    //}
}