using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using ABCinema_WPF.Controls;
using ABCinema_WPF.Models;
using ABCinema_WPF.Utils;
using MahApps.Metro.Controls.Dialogs;

namespace ABCinema_WPF.ViewModels;

public class HallViewModel : INotifyPropertyChanged
{

    public ICommand ShowHallDialogCommand { get; set; }
    
    public ICommand CloseHallDialogCommand { get; set; }
    
    public ICommand AddHallCommand { get; set; }
    
    public event PropertyChangedEventHandler? PropertyChanged;
    
    public ObservableCollection<HallItem> Halls { get; set; }
    
    private readonly IDialogCoordinator _dialogCoordinator;
    
    private readonly HallDialog _dialog;

    private HallViewModel(ObservableCollection<HallItem> halls, IDialogCoordinator dialogCoordinator)
    {
        _dialogCoordinator = dialogCoordinator;
        Halls = halls;
        _dialog = new HallDialog { DataContext = this };

        ShowHallDialogCommand = new RelayCommand(ShowDialog);
        CloseHallDialogCommand = new RelayCommand(CloseDialog);
        AddHallCommand = new RelayCommand(AddHall);
    }

    public static async Task<HallViewModel> HallViewModelFactory(IDialogCoordinator dialogCoordinator) =>
     new(new ObservableCollection<HallItem>(await HallModel.GetHallItemsAsync()), dialogCoordinator);
    

    private async Task ShowDialog(object? parameter)
        => await _dialogCoordinator.ShowMetroDialogAsync(this, _dialog);
    
    private async Task CloseDialog(object? parameter)
        => await _dialogCoordinator.HideMetroDialogAsync(this, _dialog);

    private async Task AddHall(object? parameter)
    {
        if (string.IsNullOrWhiteSpace(_dialog.HallNameBox.Text)
            || string.IsNullOrWhiteSpace(_dialog.RowBox.Text)
            || string.IsNullOrWhiteSpace(_dialog.SeatsPerRowBox.Text))
            return;

        if (!int.TryParse(_dialog.RowBox.Text,  out var row)
            || !int.TryParse(_dialog.SeatsPerRowBox.Text, out var seatsPerRow))
            return;

        await HallModel.AddHallAsync(new Hall(
            1,
            _dialog.HallNameBox.Text,
            row,
            seatsPerRow
        ));
        
        await CloseDialog(parameter);
    }

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}