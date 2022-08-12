using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace gamexDesktopApp.ViewModels
{
    public interface IGameViewModel
    {
        ICommand GamesViewCommand { get; }
        string Description { get; set; }
        ICommand RefreshGameCommand { get; }
        ICommand AccountViewCommand { get; }
        int Id { get; set; }
        ICommand LogoutCommand { get; }
        string Name { get; set; }
        decimal Price { get; set; }
        string SerialKey { get; set; }
        decimal Total { get; set; }
        BitmapImage Source { get; set; }

        void Dispose();
    }
}