using ABCinema_WPF.ViewModels;
using System.Windows;
using System.Windows.Controls;
using ABCinema_WPF.Utils;
using MahApps.Metro.Controls.Dialogs;

namespace ABCinema_WPF.Views;

public partial class AfishaView : Page
{
    public AfishaView()
    {
        InitializeComponent();
        DatePicker.SelectedDate = DateTime.Now.Date;
        DatePicker.Text = DateTime.Now.Date.ToShortDateString();
        //Loaded += AfishaView_Loaded;
    }
    
    //private async void AfishaView_Loaded(object sender, RoutedEventArgs e)
    //{
    //    var date = DatePicker.SelectedDate;
    //    DataContext = await AfishaViewModel.AfishaViewModelFactory(date.Value, DialogCoordinator.Instance);
    //}
}