using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ABCinema_WPF.Utils;

public static class ImageConverter
{
    public static ImageSource UriToImageSource(string uri)
    {
        var fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, uri);
        if (!File.Exists(fullPath)) return null;

        var bitmap = new BitmapImage();
        bitmap.BeginInit();
        bitmap.UriSource = new Uri(fullPath, UriKind.Absolute);
        bitmap.CacheOption = BitmapCacheOption.OnLoad;
        bitmap.EndInit();
        return bitmap;
    }
}