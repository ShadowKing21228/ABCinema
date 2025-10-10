using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using ABCinema_WPF.Controls;
using ABCinema_WPF.Models;
using ABCinema_WPF.Services;
using ABCinema_WPF.Utils;
using ABCinema_WPF.Views;
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
     
    public ICommand SelectSessionCommand { get; set; }
    
    public ICommand ShowUpdateSessionDialogCommand { get; }
    
    public ICommand CloseUpdateSessionDialogCommand { get; }
    
    public ICommand UpdateSessionCommand { get; }
    
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
    
    private INavigationService NavigationService { get; set; }
    
    private readonly SessionDialog _sessionDialog;
    
    private readonly SessionUpdateDialog _updateDialog;
    
    public event PropertyChangedEventHandler? PropertyChanged;
    
    //public ICommand BuyCommand = new RelayCommand<Movie>(BuyTicket);

    private AfishaViewModel(ObservableCollection<AfishaItem> afishaItems, IDialogCoordinator coordinator, INavigationService navigationService)
    {
        AfishaItems = afishaItems;
        DialogCoordinator = coordinator;
        NavigationService = navigationService;
        _updateDialog = new SessionUpdateDialog { DataContext = this };
        _sessionDialog = new SessionDialog { DataContext = this };

        ShowUpdateSessionDialogCommand = new RelayCommand(ShowUpdateDialog);
        CloseUpdateSessionDialogCommand = new RelayCommand(CloseUpdateDialog);
        UpdateSessionCommand = new RelayCommand(UpdateSession);
        TodayButtonClick = new RelayCommand(SelectTodayButtonClick);
        TomorrowButtonClick = new RelayCommand(SelectTomorrowButtonClick);
        DayAfterTomorrowButtonClick = new RelayCommand(SelectDayAfterTomorrowButtonClick);
        ShowSessionDialogCommand = new RelayCommand(ShowAddSessionDialog);
        AddSessionCommand = new RelayCommand(AddSession);
        CloseSessionDialogCommand = new RelayCommand(CloseAddSessionDialog);
        SelectSessionCommand = new RelayCommand(SelectSession);
    }
    
    public static async Task<AfishaViewModel> AfishaViewModelFactory(DateTime dateTime, IDialogCoordinator coordinator, INavigationService navigationService) 
    
        => new(await AfishaModel.GetAfishaItemsAsync(dateTime), coordinator, navigationService);
    

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
            DialogCoordinator.ShowModalMessageExternal(this, "Все поля должны быть введены", "Введите все поля корректно");
            return;
        }
        
        if (!int.TryParse(_sessionDialog.CinemaIdBox.Text, out int movieId) ||
            !int.TryParse(_sessionDialog.HallIdBox.Text, out int hallId) ||
            !decimal.TryParse(_sessionDialog.PriceBox.Text, out decimal price))
        {
            DialogCoordinator.ShowModalMessageExternal(this, "Поля Id и Цена должны быть числовыми", "Введите все поля корректно");
            return;
        }

        if (price <= 0) 
        {
            DialogCoordinator.ShowModalMessageExternal(this, "Цена не может быть отрицательной или 0", "Введите все поля корректно");
            return;
        }
        
        if (_sessionDialog.EndDateTime.SelectedDateTime.Value < _sessionDialog.StartDateTime.SelectedDateTime.Value) {
            DialogCoordinator.ShowModalMessageExternal(this, "Время конца должно быть больше времени начала", "Введите все поля корректно");
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

    private async Task SelectSession(object? parameter)
    {
        if (parameter is not Session session) return;
        
        NavigationService.Navigate(new BuyTicketView {
            DataContext = await BuyTicketViewModel.Factory(session, DialogCoordinator, NavigationService)
        });
    }
    
    private async Task ShowUpdateDialog(object? parameter)
    {
        await DialogCoordinator.ShowMetroDialogAsync(this, _updateDialog);
    }

    private async Task CloseUpdateDialog(object? parameter)
    {
        await DialogCoordinator.HideMetroDialogAsync(this, _updateDialog);
    }
    
    private async Task UpdateSession(object? parameter)
    {
        if (!int.TryParse(_updateDialog.SessionIdBox.Text, out var sessionId) ||
            !int.TryParse(_updateDialog.CinemaIdBox.Text, out var movieId) ||
            !int.TryParse(_updateDialog.HallIdBox.Text, out var hallId) ||
            !decimal.TryParse(_updateDialog.PriceBox.Text, out var price) ||
            !_updateDialog.StartDateTime.SelectedDateTime.HasValue ||
            !_updateDialog.EndDateTime.SelectedDateTime.HasValue)
        {
            DialogCoordinator.ShowModalMessageExternal(this, "Все поля должны быть введены", "Введите все поля корректно");
            return;
        }

        var updated = new Session(
            sessionId,
            movieId,
            hallId,
            _updateDialog.StartDateTime.SelectedDateTime.Value,
            _updateDialog.EndDateTime.SelectedDateTime.Value,
            price,
            false // или пересчитать по времени
        );

        await AfishaModel.UpdateSessionAsync(updated);
        await CloseUpdateDialog(parameter);
    }
    
    //protected async Task OnPropertyChanged(PropertyChangedEventArgs e)
    //{
    //    await LoadSessionsAsync();
    //    PropertyChanged?.Invoke(this, e);
    //}
}