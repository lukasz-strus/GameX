using gamexDesktopApp.Helpers;
using gamexDesktopApp.State.Accounts;
using gamexDesktopApp.ViewModels;
using gamexServices;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace gamexDesktopApp.Commands;

public class LoadImageCommand : ICommand
{
    private readonly GameAdminViewModel _gameAdminViewModel;
    private readonly IFileService _fileService;
    private readonly IAccountStore _accountStore;

    public LoadImageCommand(GameAdminViewModel gameAdminViewModel,
                            IFileService fileService,
                            IAccountStore accountStore)
    {
        _gameAdminViewModel = gameAdminViewModel;
        _fileService = fileService;
        _accountStore = accountStore;
    }

    public event EventHandler CanExecuteChanged;

    public bool CanExecute(object parameter)
    {
        return true;
    }

    public async void Execute(object parameter)
    {
        try
        {
            var token = _accountStore.CurrentAccount.Token;
            var gameId = _gameAdminViewModel.Id;
            var image = FileHelper.LoadImage(gameId);

            if (image == null)
                _gameAdminViewModel.ErrorMessage = "Błąd odczytu";

            var response = await _fileService.Create(token, image);

            if (response == (int)HttpStatusCode.OK)
                _gameAdminViewModel.ErrorMessage = "Obrazek został dodany";

            _gameAdminViewModel.GetGameCommand.Execute(null);
        }
        catch (InvalidOperationException)
        {
            _gameAdminViewModel.ErrorMessage = "Błąd odczytu";
        }
    }
}