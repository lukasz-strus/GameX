using gamexModels;
using Microsoft.Win32;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Media.Imaging;

namespace gamexDesktopApp.Helpers;

public static class FileHelper
{
    public static BitmapImage SetImage(ImageDto imageDto)
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

    public static CreateImageDto LoadImage(int gameId)
    {
        CreateImageDto imageDto = new();

        var openFileDialog = new OpenFileDialog()
        {
            Title = "Wybierz obrazek",
            Filter = ""
        };

        ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
        var separator = string.Empty;

        foreach (var c in codecs)
        {
            string codecName = c.CodecName[8..].Replace("Codec", "Files").Trim();
            openFileDialog.Filter = String.Format("{0}{1}{2} ({3})|{3}",
                                                    openFileDialog.Filter,
                                                    separator, codecName,
                                                    c.FilenameExtension);
            separator = "|";
        }
        openFileDialog.Filter = String.Format("{0}{1}{2} ({3})|{3}", openFileDialog.Filter, separator, "All Files", "*.*");
        openFileDialog.DefaultExt = ".png";

        string fileSourceName = null;

        if (openFileDialog.ShowDialog() == true)
        {
            fileSourceName = openFileDialog.FileName;
        }

        if (fileSourceName == null)
        {
            return null;
        }

        var source = File.ReadAllBytes(fileSourceName);
        var extension = Path.GetExtension(fileSourceName);

        imageDto.ImageStream = source;
        imageDto.GameId = gameId;
        imageDto.Extension = extension;

        return imageDto;
    }
}