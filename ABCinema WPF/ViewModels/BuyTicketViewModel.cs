using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using ABCinema_WPF.Models;
using ABCinema_WPF.Services;
using ABCinema_WPF.Utils;
using MahApps.Metro.Controls.Dialogs;

namespace ABCinema_WPF.ViewModels;

public class BuyTicketViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;
    public Session Session { get; set; }
    
    public Hall Hall { get; set; }

    public ObservableCollection<SeatItem> Seats { get; set; }
    
    public ICommand BuyCommand { get; set; }
    
    public ICommand ShowDialogCommand { get; set; }
    
    public ICommand HideDialogCommand { get; set; }
    
    private IDialogCoordinator DialogCoordinator { get; set; }
    
    private INavigationService NavigationService { get; set; }
    

    private BuyTicketViewModel(Session session, Hall hall, ObservableCollection<SeatItem> list, IDialogCoordinator dialogCoordinator, INavigationService navigationService)
    {
        Session = session;
        Hall = hall;
        DialogCoordinator = dialogCoordinator;
        NavigationService = navigationService;
        
        Seats = list;

        BuyCommand = new RelayCommand(BuyTicket);
        ShowDialogCommand = new RelayCommand(ShowBuyDialog);
    }

    public static async Task<BuyTicketViewModel> Factory(Session session, IDialogCoordinator dialogCoordinator, INavigationService navigationService)
    {
        var hall = await BuyTicketModel.GetHall(session.HallId);
        var seatItems = await FillSeats(session, hall);
        return new BuyTicketViewModel(session, hall, seatItems ,dialogCoordinator, navigationService);
    }

    private static async Task<ObservableCollection<SeatItem>> FillSeats(Session session, Hall hall)
    {
        ObservableCollection<SeatItem> seats = [];
        
        var var = await BuyTicketModel.GetReservationSeats(session);
        
        for (var r = 0; r < hall.Rows; r++) {
            
            for (var s = 0; s < hall.SeatsPerRow; s++)
            {
                var isReserved = var.Any(seat => seat.RowNumber == r && seat.SeatNumber == s);
                seats.Add(new SeatItem(r, s, isReserved));
            }
            
        }

        return seats;
    }

    private async Task BuyTicket(object? obj)
    {
        if (obj is SeatItem seat)
            await BuyTicketModel.AddReservation(Session, seat);
    }
    
    private async Task ShowBuyDialog(object? obj)
    {
        if (obj is SeatItem seat) {
            var dialogResult = await DialogCoordinator.ShowMessageAsync(this, "Подтверждение покупки", $"Билет {seat.RowNumber + 1} x {seat.SeatNumber + 1}");
            
            if (dialogResult == MessageDialogResult.Affirmative)
                await BuyTicket(seat);
        }
    }
}