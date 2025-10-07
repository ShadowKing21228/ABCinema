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
    
    public ICommand ShowHallUpdateDialogCommand { get; }
    
    public ICommand CloseHallUpdateDialogCommand { get; }

    public ICommand HallUpdateCommand { get; }

    public event PropertyChangedEventHandler? PropertyChanged;
    
    public ObservableCollection<HallItem> Halls { get; set; }
    
    public HallItem? SelectedHall
    {
        get => _selectedHall;
        set
        {
            _selectedHall = value;
            OnPropertyChanged();
        }
    }

    private HallItem? _selectedHall;
    
    private readonly IDialogCoordinator _dialogCoordinator;
    
    private readonly HallDialog _dialog;
    
    private readonly HallUpdateDialog _updateDialog;


    private HallViewModel(ObservableCollection<HallItem> halls, IDialogCoordinator dialogCoordinator)
    {
        _dialogCoordinator = dialogCoordinator;
        Halls = halls;
        _dialog = new HallDialog { DataContext = this };
        _updateDialog = new HallUpdateDialog { DataContext = this };

        ShowHallDialogCommand = new RelayCommand(ShowDialog);
        CloseHallDialogCommand = new RelayCommand(CloseDialog);
        AddHallCommand = new RelayCommand(AddHall);
        ShowHallUpdateDialogCommand = new RelayCommand(ShowUpdateDialog);
        CloseHallUpdateDialogCommand = new RelayCommand(CloseUpdateDialog);
        HallUpdateCommand = new RelayCommand(UpdateHall);
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
    
    private async Task ShowUpdateDialog(object? parameter)
    {
        if (SelectedHall == null) return;

        _updateDialog.NameBox.Text = SelectedHall.Name;
        _updateDialog.RowBox.Text = SelectedHall.Rows.ToString();
        _updateDialog.SeatsPerBox.Text = SelectedHall.SeatsPerRow.ToString();

        await _dialogCoordinator.ShowMetroDialogAsync(this, _updateDialog);
    }

    private async Task CloseUpdateDialog(object? parameter)
        => await _dialogCoordinator.HideMetroDialogAsync(this, _updateDialog);

    
    private async Task UpdateHall(object? parameter)
    {
        if (SelectedHall == null) return;

        var name = _updateDialog.NameBox.Text;
        if (string.IsNullOrWhiteSpace(name)) return;

        if (!int.TryParse(_updateDialog.RowBox.Text, out var rows)
            || !int.TryParse(_updateDialog.SeatsPerBox.Text, out var seatsPerRow))
            return;

        var updated = SelectedHall with
        {
            Name = name,
            Rows = rows,
            SeatsPerRow = seatsPerRow
        };

        await HallModel.UpdateHallAsync(new Hall(updated.Id, updated.Name, updated.Rows, updated.SeatsPerRow));

        var index = Halls.IndexOf(SelectedHall);
        if (index >= 0)
            Halls[index] = updated;

        SelectedHall = updated;

        await CloseUpdateDialog(parameter);
    }

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}