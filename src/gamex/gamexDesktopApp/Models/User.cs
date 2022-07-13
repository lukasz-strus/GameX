using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace gamexDesktopApp.Models;

public class User : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;

    private int _id;

    public int Id
    {
        get => _id;
        set
        {
            _id = value;
            OnPropertyChanged();
        }
    }

    private string _login;

    public string Login
    {
        get => _login;
        set
        {
            _login = value;
            OnPropertyChanged();
        }
    }

    private string _password;

    public string Password
    {
        get => _password;
        set
        {
            _password = value;
            OnPropertyChanged();
        }
    }

    private string _email;

    public string Email
    {
        get => _email;
        set
        {
            _email = value;
            OnPropertyChanged();
        }
    }

    private decimal _total;

    public decimal Total
    {
        get => _total;
        set
        {
            _total = value;
            OnPropertyChanged();
        }
    }

    private Role _role;

    public Role Role
    {
        get => _role;
        set
        {
            _role = value;
            OnPropertyChanged();
        }
    }

    private void OnPropertyChanged([CallerMemberName] string name = null) =>
    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}