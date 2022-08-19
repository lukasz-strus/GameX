using gamexModels;
using gamexServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace gamexDesktopApp.ViewModels;

public interface IPagesViewModel
{
    int ItemsFrom { get; set; }
    int ItemsTo { get; set; }
    int PageNumber { get; set; }
    int PageSize { get; set; }
    string SearchPhrase { get; set; }
    int TotalItemsCount { get; set; }
    int TotalPages { get; set; }
    SortGameBy SortBy { get; set; }
    SortDirection SortDirection { get; set; }
    ICommand RefreshGamesCommand { get; }
    ICommand UpdatePageCommand { get; }
    ICommand UpdatePageSizeCommand { get; }
    string ErrorMessage { set; }
    MessageViewModel ErrorMessageViewModel { get; }
}