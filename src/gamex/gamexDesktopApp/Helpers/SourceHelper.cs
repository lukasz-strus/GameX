using System.Drawing;
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

    private static bool IsImageSourceExists(string source) =>
        File.Exists(source);
}