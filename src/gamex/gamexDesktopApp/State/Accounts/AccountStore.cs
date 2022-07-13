using gamexDesktopApp.Models;

namespace gamexDesktopApp.State.Accounts;

public interface IAccountStore
{
    Account CurrentAccount { get; set; }

    event Action StateChanged;
}

public class AccountStore : IAccountStore
{
    private Account _currentAccount;

    public Account CurrentAccount
    {
        get
        {
            return _currentAccount;
        }
        set
        {
            _currentAccount = value;
            StateChanged?.Invoke();
        }
    }

    public event Action StateChanged;
}