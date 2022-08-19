using gamexDesktopApp.State.Accounts;
using gamexDesktopApp.ViewModels;
using gamexModels;
using gamexServices;
using System.Net;

namespace gamexDesktopApp.Commands;

public class ChangePasswordCommand : AsyncCommandBase

{
    private readonly IPasswordViewModel _accountViewModel;
    private readonly IAuthenticationService _authenticationService;
    private readonly IAccountStore _accountStore;

    public ChangePasswordCommand(IPasswordViewModel accountViewModel,
                                 IAuthenticationService authenticationService,
                                 IAccountStore accountStore)
    {
        _accountViewModel = accountViewModel;
        _authenticationService = authenticationService;
        _accountStore = accountStore;
    }

    public override async Task ExecuteAsync(object parameter)
    {
        try
        {
            UserPasswordDto passwordDto = new()
            {
                Password = _accountViewModel.Password,
                ConfirmPassword = _accountViewModel.ConfirmPassword
            };

            var token = _accountStore.CurrentAccount.Token;

            var response = await _authenticationService.ChangePassword(token, passwordDto);

            if (response == (int)HttpStatusCode.OK)
            {
                _accountViewModel.ErrorMessage = "Successful password change";
            }
        }
        catch (Exception)
        {
            _accountViewModel.ErrorMessage = "Something goes wrong";
        }
    }
}