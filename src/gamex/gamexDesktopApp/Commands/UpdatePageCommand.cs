using gamexDesktopApp.ViewModels;
using System.Windows.Input;

namespace gamexDesktopApp.Commands;

public class UpdatePageCommand<T> : ICommand
    where T : IPagesViewModel
{
    private readonly T _viewModel;

    public UpdatePageCommand(T viewModel)
    {
        _viewModel = viewModel;
    }

    public event EventHandler CanExecuteChanged;

    public bool CanExecute(object parameter)
    {
        return true;
    }

    public void Execute(object parameter)
    {
        try
        {
            if (parameter.ToString() == "Next")
            {
                _viewModel.PageNumber++;
            }
            else
            {
                _viewModel.PageNumber = int.Parse(parameter.ToString());
            }
            _viewModel.ViewListCommand.Execute(_viewModel);
            _viewModel.ViewListCommand.Execute(_viewModel);
        }
        catch (Exception)
        {
            _viewModel.ErrorMessage = "Failed.";
        }
    }
}