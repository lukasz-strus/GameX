using gamexDesktopApp.Helpers;
using gamexDesktopApp.ViewModels;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace gamexDesktopApp.Commands;

public class LoadImageCommand : ICommand
{
    private readonly GameAdminViewModel _gameAdminViewModel;

    public LoadImageCommand(GameAdminViewModel gameAdminViewModel)
    {
        _gameAdminViewModel = gameAdminViewModel;
    }

    public event EventHandler CanExecuteChanged;

    public bool CanExecute(object parameter)
    {
        return true;
    }

    public void Execute(object parameter)
    {
        try
        {
            var projectDirectory = FileHelper.GetProjectDirectory();
            var copyName = _gameAdminViewModel.Id;
            var fullCopyPath = $"{projectDirectory}/Images/Games/{copyName}";

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

            if (openFileDialog.ShowDialog() == true)
            {
                var fileSourceName = openFileDialog.FileName;
                var extension = Path.GetExtension(fileSourceName);

                //TODO dodać exception do innych typów, np pdf, txt itd.

                File.Copy(fileSourceName, string.Concat(fullCopyPath, extension), true);
            }

            _gameAdminViewModel.GetGameCommand.Execute(null);
        }
        catch (InvalidOperationException)
        {
            _gameAdminViewModel.ErrorMessage = "Błąd odczytu";
        }
    }
}