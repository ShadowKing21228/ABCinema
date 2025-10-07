using System.Collections.ObjectModel;
using System.ComponentModel;
using ABCinema_WPF.Controls;
using ABCinema_WPF.Services;
using ABCinema_WPF.Views;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;

namespace ABCinema_WPF.ViewModels;

public class MainWindowViewModel
{
    private static INavigationService _navigationService;

    public ObservableCollection<HamburgerMenuGlyphItem> MenuItems { get; set; }
    
    private MainWindowViewModel(INavigationService navigationService)
    {
        MenuItems = [];
        _navigationService = navigationService;
    }

    public static async Task<MainWindowViewModel> CreateAsync(INavigationService navigationService)
    {
        var vm = new MainWindowViewModel(navigationService);
        await FillMenuItems(vm);
        return vm;
    }

    private static async Task FillMenuItems(MainWindowViewModel vm)
    {
        var afishaItem = new HamburgerMenuGlyphItemDc {
            Label = "Афиша",
            Glyph = "📜",
            Tag = new AfishaView()
        };
        
        await afishaItem.InitViewModelAsync(async view => 
            await AfishaViewModel.AfishaViewModelFactory(DateTime.Now.Date, DialogCoordinator.Instance, _navigationService));
        
        
        var cinemaItem = new HamburgerMenuGlyphItemDc {
            Label = "Фильмы",
            Glyph = "🎬",
            Tag = new CinemaView()
        };
        
        await cinemaItem.InitViewModelAsync(async view =>
            await CinemaViewModel.CreateAsync(DialogCoordinator.Instance));

        var hallItem = new HamburgerMenuGlyphItemDc
        {
            Label = "Залы",
            Glyph = "🏟️",
            Tag = new HallView()
        };
        
        await hallItem.InitViewModelAsync(async view =>
            await HallViewModel.HallViewModelFactory(DialogCoordinator.Instance));
        
        var genreItem = new HamburgerMenuGlyphItemDc
        {
            Label = "Жанры",
            Glyph = "😱",
            Tag = new GenreView()
        }; 
        
        await genreItem.InitViewModelAsync(async view => 
            await GenreViewModel.GenreViewModelFactory(DialogCoordinator.Instance));
        
        vm.MenuItems.Add(afishaItem);
        vm.MenuItems.Add(cinemaItem);
        vm.MenuItems.Add(hallItem);
        vm.MenuItems.Add(genreItem);
    }
    public event PropertyChangedEventHandler? PropertyChanged;
}