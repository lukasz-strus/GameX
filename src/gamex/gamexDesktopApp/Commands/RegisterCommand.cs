using gamexDesktopApp.State.Authenticators;
using gamexDesktopApp.State.Navigators;
using gamexDesktopApp.ViewModels;
using System.ComponentModel;

namespace gamexDesktopApp.Commands;

public class RegisterCommand : AsyncCommandBase
{
    private readonly RegistrationViewModel _registrationViewModel;
    private readonly IAuthenticator _authenticator;
    private readonly IRenavigator _registerRenavigator;

    public RegisterCommand(RegistrationViewModel registrationViewModel, IAuthenticator authenticator, IRenavigator registerRenavigator)
    {
        _registrationViewModel = registrationViewModel;
        _authenticator = authenticator;
        _registerRenavigator = registerRenavigator;

        _registrationViewModel.PropertyChanged += RegistrationViewModel_PropertyChanged;
    }

    public override bool CanExecute(object parameter)
    {
        return _registrationViewModel.CanRegister && base.CanExecute(parameter);
    }

    public override async Task ExecuteAsync(object parameter)
    {
        _registrationViewModel.ErrorMessage = string.Empty;

        try
        {
            var registrationResult = await _authenticator.Register(
                _registrationViewModel.Login,
                _registrationViewModel.Password,
                _registrationViewModel.ConfirmPassword,
                _registrationViewModel.Email,
                _registrationViewModel.ConfirmEmail);

            switch (registrationResult)
            {
                case 200:
                    _registerRenavigator.Renavigate();
                    break;

                default:
                    _registrationViewModel.ErrorMessage = "Registration failed.";
                    break;
            }
        }
        catch (Exception)
        {
            _registrationViewModel.ErrorMessage = "Registration failed.";
        }
    }

    private void RegistrationViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(RegistrationViewModel.CanRegister))
        {
            OnCanExecuteChanged();
        }
    }
}