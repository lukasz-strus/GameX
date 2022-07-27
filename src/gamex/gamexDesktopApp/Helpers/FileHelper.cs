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

    public static BitmapImage LoadImage(byte[] imageData)
    {
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

    public static string SearchFilePath(string pathToSearch, string partialName)
    {
        var hdDirectoryInWhichToSearch = new DirectoryInfo(pathToSearch);
        FileInfo[] filesInDir = hdDirectoryInWhichToSearch.GetFiles("*" + partialName + "*.*");

        if (filesInDir.Length == 0)
        {
            var fileName = "0.jpg";
            return Path.Combine(pathToSearch, fileName);
        }

        return filesInDir.FirstOrDefault().FullName;
    }

    public static string GetProjectDirectory() =>
        Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;

    public static void DeleteGamesImages()
    {
        var currentPath = FileHelper.GetProjectDirectory();
        var fullpath = string.Concat(currentPath, $"/Images/Games/");

        DirectoryInfo di = new DirectoryInfo(fullpath);

        foreach (FileInfo file in di.GetFiles())
        {
            file.Delete();
        }
        foreach (DirectoryInfo dir in di.GetDirectories())
        {
            dir.Delete(true);
        }
    }
}