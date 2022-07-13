using System.Collections.ObjectModel;

namespace gamexDesktopApp.Models
{
    public class Games
    {
        public ObservableCollection<Game> GamesCollection { get; set; } =
            new ObservableCollection<Game>();
    }
}