using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using ABCinema_WPF.Models;
using ABCinema_WPF.ViewModels;
using MahApps.Metro.Controls.Dialogs;
using Microsoft.Win32;

namespace ABCinema_WPF.Controls;

public partial class CinemaDialog : CustomDialog
{
    private string? _selectedImagePath;
    
    public string? GetSelectedImagePath() => _selectedImagePath;

    public CinemaDialog()
    {
        InitializeComponent();
    }
    
    private void SelectImage_Click(object sender, RoutedEventArgs e)
    {
        var dialog = new OpenFileDialog
        {
            Filter = "Изображения (*.png;*.jpg;*.jpeg)|*.png;*.jpg;*.jpeg"
        };

        if (dialog.ShowDialog() != true) return;
        
        _selectedImagePath = dialog.FileName;
        PosterPreview.Source = new BitmapImage(new Uri(_selectedImagePath));
    }

    private void GenreListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (DataContext is not MovieViewModel vm || vm.SelectedGenres is null)
            return;

        vm.SelectedGenres.Clear();
        foreach (Genre genre in GenreListBox.SelectedItems)
        {
            vm.SelectedGenres.Add(genre);
        }
    }

    private void ForceOpenFlyout(object sender, RoutedEventArgs e)
    {
        GenreFlyout.IsOpen = true;
    }
}