using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace gamexDesktopApp.Models;

/// <summary>
/// Game Class
/// </summary>
public class Game : INotifyPropertyChanged
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

    private string _name;

    public string Name
    {
        get => _name;
        set
        {
            _name = value;
            OnPropertyChanged();
        }
    }

    private string _description;

    public string Description
    {
        get => _description;
        set
        {
            _description = value;
            OnPropertyChanged();
        }
    }

    private decimal _price;

    public decimal Price
    {
        get => _price;
        set
        {
            _price = value;
            OnPropertyChanged();
        }
    }

    private void OnPropertyChanged([CallerMemberName] string name = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}