using System.Collections.ObjectModel;

namespace gamexDesktopApp.Models;

public class Users
{
    public ObservableCollection<User> UsersCollection { get; set; } =
            new ObservableCollection<User>();
}