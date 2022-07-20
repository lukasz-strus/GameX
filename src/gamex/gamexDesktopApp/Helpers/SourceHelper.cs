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
        var currentPath = GetProjectDirectory();

        var fullpath = string.Concat(currentPath, $"/Images/Games/{id}.jpg");
        var defaultPath = string.Concat(currentPath, $"/Images/Games/0.jpg");

        return IsImageSourceExists(fullpath) ? fullpath : defaultPath;
    }

    public static string GetFilePath(string pathToSearch, string partialName)
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