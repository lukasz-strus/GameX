using gamexDesktopApp.State.Accounts;
using gamexDesktopApp.State.Selected;
using gamexDesktopApp.ViewModels;
using gamexServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gamexDesktopApp.Commands;

public class DeleteCommand : AsyncCommandBase
{
    private readonly IPagesViewModel _viewModel;
    private readonly ISelected _selected;
    private readonly IDeleteService _deleteService;
    private readonly IAccountStore _accountStore;

    public DeleteCommand(IPagesViewModel gameViewModel,
                         ISelected selected,
                         IDeleteService deleteService,
                         IAccountStore accountStore)
    {
        _viewModel = gameViewModel;
        _selected = selected;
        _deleteService = deleteService;
        _accountStore = accountStore;
    }

    public override bool CanExecute(object parameter)
    {
        return true;
    }

    public override async Task ExecuteAsync(object parameter)
    {
        try
        {
            var token = _accountStore.CurrentAccount.Token;
            int id = (int)_selected.Id;
            await _deleteService.Delete(token, id);
            _viewModel.ViewListCommand.Execute(null);
        }
        catch (Exception)
        {
            _viewModel.ErrorMessage = "Coś poszło nie tak";
        }
    }
}