using System.Collections.ObjectModel;
using System.ComponentModel;
using ABCinema_WPF.Views;
using MahApps.Metro.Controls;

namespace ABCinema_WPF.ViewModels;

public class MainWindowViewModel
{
    public ObservableCollection<HamburgerMenuGlyphItem> MenuItems { get; set; } = new()
    {
        new HamburgerMenuGlyphItem { Label = "Афиша", Glyph = "📜", Tag = typeof(AfishaView) },
        new HamburgerMenuGlyphItem { Label = "Фильмы", Glyph = "🎬", Tag = typeof(CinemaView) },
        new HamburgerMenuGlyphItem { Label = "Залы", Glyph = "🏟️", Tag = typeof(HallView) }
    };
    
    public event PropertyChangedEventHandler? PropertyChanged;
}