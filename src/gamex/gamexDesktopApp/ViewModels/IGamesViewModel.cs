using gamexDesktopApp.Models;
using gamexServices;
using System.ComponentModel;
using System.Windows.Input;

namespace gamexDesktopApp.ViewModels
{
    public interface IGamesViewModel : IPagesViewModel
    {
        Games Games { get; set; }
        ICollectionView GamesListView { get; }
        Game Selected { get; set; }
        decimal Total { get; set; }

        void Dispose();
    }
}