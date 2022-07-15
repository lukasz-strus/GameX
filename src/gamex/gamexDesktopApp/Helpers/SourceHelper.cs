using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gamexDesktopApp.Helpers;

public static class SourceHelper
{
    public static string SetSource(int id)
    {
        var imagePath = $"/Images/Games/{id}.jpg";
        var fullpath = string.Concat(GetProjectDirectory(), imagePath);

        return IsImageSourceExists(imagePath) ? fullpath : null;
    }

    public static string GetProjectDirectory() =>
        Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;

    private static bool IsImageSourceExists(string source) =>
        File.Exists(string.Concat(GetProjectDirectory(), source));
}