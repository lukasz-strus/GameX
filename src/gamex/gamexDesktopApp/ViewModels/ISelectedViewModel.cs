namespace gamexDesktopApp.ViewModels;

public interface ISelectedViewModel
{
    string ErrorMessage { set; }
    MessageViewModel ErrorMessageViewModel { get; }
}