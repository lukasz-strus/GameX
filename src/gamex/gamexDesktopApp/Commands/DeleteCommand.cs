using gamexDesktopApp.State.Accounts;
using gamexDesktopApp.State.Selected;
using gamexDesktopApp.ViewModels;
using gamexServices;
using System.Net;

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
            var id = (int)_selected.Id;
            var response = await _deleteService.Delete(token, id);

            var record = GetRecordType();

            if (response == (int)HttpStatusCode.OK)
            {
                _viewModel.ErrorMessage = $"The {record} was successfully deleted";
            }

            _viewModel.RefreshGamesCommand.Execute(null);
        }
        catch (Exception)
        {
            _viewModel.ErrorMessage = "Something went wrong";
        }
    }

    private string GetRecordType() => _selected switch
    {
        ISingleGame => "game",
        ISingleUser => "user",
        _ => "item",
    };
}