using gamexDesktopApp.ViewModels;
using System.Windows.Input;

namespace gamexDesktopApp.Commands;

public class UpdatePageSizeCommand<T> : ICommand
    where T : IPagesViewModel
{
    private readonly T _viewModel;

    public UpdatePageSizeCommand(T viewModel)
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
            _viewModel.PageSize = int.Parse(parameter.ToString());
            _viewModel.UpdatePageCommand.Execute("Start");
        }
        catch (Exception)
        {
            _viewModel.ErrorMessage = "Failed.";
        }
    }
}