using gamexDesktopApp.Models;
using gamexDesktopApp.State.Accounts;
using gamexDesktopApp.ViewModels;
using gamexModels;
using gamexServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gamexDesktopApp.Commands
{
    public class GetUsersListCommand : AsyncCommandBase
    {
        private readonly UsersViewModel _usersViewModel;
        private readonly IUserService _userService;
        private GetAllResult<UserDto> _getAllResult;
        private readonly IAccountStore _accountStore;

        public GetUsersListCommand(UsersViewModel usersViewModel,
                                   IUserService userService,
                                   IAccountStore accountStore)
        {
            _usersViewModel = usersViewModel;
            _userService = userService;
            _accountStore = accountStore;
        }

        public override async Task ExecuteAsync(object parameter)
        {
            try
            {
                _usersViewModel.Users.UsersCollection.Clear();
                _getAllResult = await GetAllResult();
                Mapping(_getAllResult);
            }
            catch (Exception)
            {
                _usersViewModel.ErrorMessage = "Failed";
            }
        }

        private async Task<GetAllResult<UserDto>> GetAllResult() =>
            await _userService.GetAll(_accountStore.CurrentAccount.Token, GetAllQuery());

        private GetAllQuery GetAllQuery() =>
            new()
            {
                SearchPhrase = _usersViewModel.SearchPhrase,
                PageNumber = _usersViewModel.PageNumber,
                PageSize = _usersViewModel.PageSize,
                SortBy = _usersViewModel.SortBy.ToString(),
                SortDirection = _usersViewModel.SortDirection
            };

        private void Mapping(GetAllResult<UserDto> getAllResult)
        {
            AddToGamesCollection(getAllResult.Items);
            _usersViewModel.TotalPages = getAllResult.TotalPages;
            _usersViewModel.ItemsFrom = getAllResult.ItemsFrom;
            _usersViewModel.ItemsTo = getAllResult.ItemsTo;
            _usersViewModel.TotalItemsCount = getAllResult.TotalItemsCount;
            _usersViewModel.Total = _accountStore.CurrentAccount.Total;
        }

        private void AddToGamesCollection(List<UserDto> dto)
        {
            foreach (var item in dto)
                _usersViewModel.Users.UsersCollection.Add(MapFromUserDto(item));
        }

        private User MapFromUserDto(UserDto dto) =>
            new()
            {
                Id = dto.Id,
                Login = dto.Login,
                Email = dto.Email,
                Role = (Role)dto.RoleId,
                Total = dto.Total
            };
    }
}