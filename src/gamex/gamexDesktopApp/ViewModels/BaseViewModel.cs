using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace gamexDesktopApp.ViewModels;

public delegate TViewModel CreateViewModel<TViewModel>() where TViewModel : BaseViewModel;

public class BaseViewModel : INotifyPropertyChanged
{
    public virtual void Dispose()
    {
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string name = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}