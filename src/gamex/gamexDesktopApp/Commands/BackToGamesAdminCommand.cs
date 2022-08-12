using gamexDesktopApp.Models;
using gamexDesktopApp.State.Accounts;
using gamexDesktopApp.State.Navigators;
using System.Windows.Input;

namespace gamexDesktopApp.Commands
{
    public class BackToGamesAdminCommand : ICommand
    {
        private readonly IRenavigator _adminRenavigator;
        private readonly IRenavigator _salesRenavigator;
        private readonly IAccountStore _accountStore;

        public BackToGamesAdminCommand(IRenavigator gamesAdminRenavigator,
                                       IRenavigator gamesSalesRenavigator,
                                       IAccountStore accountStore)
        {
            _adminRenavigator = gamesAdminRenavigator;
            _salesRenavigator = gamesSalesRenavigator;
            _accountStore = accountStore;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            if (_accountStore.CurrentAccount.Role == Role.Admin)
                _adminRenavigator.Renavigate();
            else
                _salesRenavigator.Renavigate();
        }
    }
}