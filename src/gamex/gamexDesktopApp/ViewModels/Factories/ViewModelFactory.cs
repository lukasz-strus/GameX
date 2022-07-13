using gamexDesktopApp.State.Navigators;

namespace gamexDesktopApp.ViewModels.Factories
{
    public interface IViewModelFactory
    {
        BaseViewModel CreateViewModel(ViewType viewType);
    }

    public class ViewModelFactory : IViewModelFactory
    {
        private readonly CreateViewModel<AccountViewModel> _createAccountViewModel;
        private readonly CreateViewModel<GamesViewModel> _createGamesViewModel;
        private readonly CreateViewModel<GameViewModel> _createGameViewModel;
        private readonly CreateViewModel<LoginViewModel> _createLoginViewModel;
        private readonly CreateViewModel<PasswordReminderViewModel> _createPasswordReminderViewModel;
        private readonly CreateViewModel<RegistrationViewModel> _createRegistrationViewModel;
        private readonly CreateViewModel<UsersViewModel> _createUsersViewModel;
        private readonly CreateViewModel<UserViewModel> _createUserViewModel;

        public ViewModelFactory(CreateViewModel<AccountViewModel> createAccountViewModel,
            CreateViewModel<GamesViewModel> createGamesViewModel,
            CreateViewModel<GameViewModel> createGameViewModel,
            CreateViewModel<LoginViewModel> createLoginViewModel,
            CreateViewModel<PasswordReminderViewModel> createPasswordReminderViewModel,
            CreateViewModel<RegistrationViewModel> createRegistrationViewModel,
            CreateViewModel<UsersViewModel> createUsersViewModel,
            CreateViewModel<UserViewModel> createUserViewModel)
        {
            _createAccountViewModel = createAccountViewModel;
            _createGamesViewModel = createGamesViewModel;
            _createGameViewModel = createGameViewModel;
            _createLoginViewModel = createLoginViewModel;
            _createPasswordReminderViewModel = createPasswordReminderViewModel;
            _createRegistrationViewModel = createRegistrationViewModel;
            _createUsersViewModel = createUsersViewModel;
            _createUserViewModel = createUserViewModel;
        }

        public BaseViewModel CreateViewModel(ViewType viewType)
        {
            return viewType switch
            {
                ViewType.Account => _createAccountViewModel(),
                ViewType.Games => _createGamesViewModel(),
                ViewType.Game => _createGameViewModel(),
                ViewType.Login => _createLoginViewModel(),
                ViewType.PasswordReminder => _createPasswordReminderViewModel(),
                ViewType.Registration => _createRegistrationViewModel(),
                ViewType.Users => _createUsersViewModel(),
                ViewType.User => _createUserViewModel(),
                _ => throw new ArgumentException("The ViewType does not have a ViewModel.", "viewType"),
            };
        }
    }
}