using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gamexDesktopApp.Helpers;

public static class ImageSourceHelper
{
    public static string SetSource(int id)
    {
        var imagePath = $"/Images/Games/{id}.jpg";

        return IsImageSourceExists(imagePath) ? imagePath : null;
    }

    private static bool IsImageSourceExists(string source)
    {
        var workingDirectory = Environment.CurrentDirectory;
        var projectDirectory = Directory.GetParent(workingDirectory).Parent.Parent.FullName;
        var fullImagePath = string.Concat(projectDirectory, source);

        return File.Exists(fullImagePath);
    }
}