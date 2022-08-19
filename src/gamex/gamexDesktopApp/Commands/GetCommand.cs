using gamexDesktopApp.Helpers;
using gamexDesktopApp.Models;
using gamexDesktopApp.State.Accounts;
using gamexDesktopApp.State.Selected;
using gamexDesktopApp.ViewModels;
using gamexModels;
using gamexServices;

namespace gamexDesktopApp.Commands;

public class GetCommand<T> : AsyncCommandBase
    where T : IDto
{
    private readonly ISelectedViewModel _selectedVewModel;
    private readonly IGetService<T> _getService;
    private readonly IAccountStore _accountStore;
    private readonly ISelected _selected;
    private readonly IFileService _fileService;

    public GetCommand(ISelectedViewModel selectedVewModel,
                      IGetService<T> getService,
                      IAccountStore accountStore,
                      ISelected selected,
                      IFileService fileService = null)
    {
        _selectedVewModel = selectedVewModel;
        _getService = getService;
        _accountStore = accountStore;
        _selected = selected;
        _fileService = fileService;
    }

    public override async Task ExecuteAsync(object parameter)
    {
        try
        {
            if (_selected != null)
            {
                var token = _accountStore.CurrentAccount.Token;
                var id = (int)_selected.Id;
                var dto = await _getService.Get(token, id);
                ImageDto image = null;

                if (_fileService != null)
                {
                    image = await _fileService.GetGameImage(token, id);
                }

                AssingProperties(dto, _selectedVewModel, image);
            }
        }
        catch (Exception)
        {
            _selectedVewModel.ErrorMessage = "Something goes wrong";
        }
    }

    private void AssingProperties(T dto, ISelectedViewModel selectedViewModel, ImageDto image)
    {
        var gameDto = dto as GameDto;
        var gameViewModel = _selectedVewModel as IGameViewModel;
        var userDto = dto as UserDto;
        var userViewModel = _selectedVewModel as UserViewModel;

        if (IsGame(gameViewModel, gameDto))
        {
            AssignGameProperties(gameViewModel, gameDto, image);
        }
        else if (IsUser(userViewModel, userDto))
        {
            AssignUserProperties(userViewModel, userDto);
        }
        else
        {
            _selectedVewModel.ErrorMessage = "Something goes wrong";
        }
    }

    private static bool IsGame(IGameViewModel gameViewModel, GameDto gameDto) =>
        gameViewModel != null && gameDto != null;

    private static bool IsUser(UserViewModel userViewModel, UserDto userDto) =>
    userViewModel != null && userDto != null;

    private void AssignGameProperties(IGameViewModel gameViewModel, GameDto gameDto, ImageDto image)
    {
        gameViewModel.Id = gameDto.Id;
        gameViewModel.Name = gameDto.Name;
        gameViewModel.Description = gameDto.Description;
        gameViewModel.Price = gameDto.Price;
        gameViewModel.Total = _accountStore.CurrentAccount.Total;
        gameViewModel.Source = FileHelper.SetImage(image);
    }

    private static void AssignUserProperties(UserViewModel userViewModel, UserDto userDto)
    {
        userViewModel.Id = userDto.Id;
        userViewModel.Login = userDto.Login;
        userViewModel.Email = userDto.Email;
        userViewModel.Role = (Role)userDto.RoleId;
    }
}