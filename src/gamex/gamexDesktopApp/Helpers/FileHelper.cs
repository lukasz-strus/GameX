using gamexModels;
using System.IO;
using System.Windows.Media.Imaging;

namespace gamexDesktopApp.Helpers;

public static class FileHelper
{
    public static BitmapImage SetSource(int id)
    {
        var currentPath = GetProjectDirectory();

        var fullpath = string.Concat(currentPath, $"/Images/Games/{id}.jpg");

        if (!File.Exists(fullpath))
        {
            return null;
        }

        var bitmapImage = new BitmapImage();
        var stream = File.OpenRead(fullpath);

        bitmapImage.BeginInit();
        bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
        bitmapImage.StreamSource = stream;
        bitmapImage.EndInit();
        stream.Close();
        stream.Dispose();

        return bitmapImage;
    }

    public static BitmapImage LoadImage(ImageDto imageDto)
    {
        if (imageDto == null)
        {
            return null;
        }

        var imageData = imageDto.ImageStream;

        if (imageData == null || imageData.Length == 0)
        {
            return null;
        }

        var image = new BitmapImage();
        using (var mem = new MemoryStream(imageData))
        {
            mem.Position = 0;
            image.BeginInit();
            image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
            image.CacheOption = BitmapCacheOption.OnLoad;
            image.UriSource = null;
            image.StreamSource = mem;
            image.EndInit();
        }

        image.Freeze();
        return image;
    }

    public static string GetProjectDirectory() =>
        Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
}