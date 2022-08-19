using gamexDesktopApp.State.Accounts;
using gamexDesktopApp.State.Selected;
using gamexDesktopApp.ViewModels;
using gamexServices;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Windows;

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
            var record = GetRecordType(_selected);

            ValidateUser(token, _selected);

            if (MessageBox.Show("Are you sure?", "Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                var response = await _deleteService.Delete(token, id);

                if (response == (int)HttpStatusCode.OK)
                {
                    _viewModel.ErrorMessage = $"The {record} was successfully deleted";
                }

                _viewModel.RefreshGamesCommand.Execute(null);
            }
        }
        catch (Exception)
        {
            _viewModel.ErrorMessage = "Something went wrong";
        }
    }

    private static string GetRecordType(ISelected selected) => selected switch
    {
        ISingleGame => "game",
        ISingleUser => "user",
        _ => "item",
    };

    private static void ValidateUser(string token, ISelected selected)
    {
        if (selected is ISingleUser user
            && user.Id == GetUserIdFromJwt(token))
        {
            MessageBox.Show("You cannot remove yourself!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            throw new Exception();
        }
    }

    private static int GetUserIdFromJwt(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        var jwtSecurityToken = handler.ReadJwtToken(token);

        var id = jwtSecurityToken.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;

        return int.Parse(id);
    }
}