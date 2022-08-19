using gamexDesktopApp.ViewModels;
using System.Windows.Input;
using System.Windows.Markup.Localizer;

namespace gamexDesktopApp.Commands;

public class UpdatePageCommand<T> : ICommand
    where T : IPagesViewModel
{
    private readonly T _viewModel;

    public UpdatePageCommand(T viewModel)
    {
        _viewModel = viewModel;
        //RaiseCanExecuteChanged();
    }

    public event EventHandler CanExecuteChanged;

    public bool CanExecute(object parameter)
    {
        switch (parameter.ToString())
        {
            case "Next":
                if (_viewModel.TotalItemsCount > (_viewModel.PageSize * _viewModel.PageNumber))
                    return true;
                break;

            case "Previous":
                if (_viewModel.PageNumber > 1)
                    return true;
                break;

            case "Start":
                return true;
        }

        return false;
    }

    public void Execute(object parameter)
    {
        try
        {
            switch (parameter.ToString())
            {
                case "Next":
                    _viewModel.PageNumber++;
                    RaiseCanExecuteChanged();
                    break;

                case "Previous":
                    _viewModel.PageNumber--;
                    RaiseCanExecuteChanged();
                    break;

                case "Start":
                    _viewModel.PageNumber = 1;
                    RaiseCanExecuteChanged();
                    break;
            }

            _viewModel.RefreshGamesCommand.Execute(_viewModel);
        }
        catch (Exception)
        {
            _viewModel.ErrorMessage = "Failed.";
        }
    }

    public void RaiseCanExecuteChanged()
    {
        CanExecuteChanged?.Invoke(this, new EventArgs());
    }
}