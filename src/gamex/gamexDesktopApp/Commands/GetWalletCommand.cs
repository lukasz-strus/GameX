using gamexDesktopApp.Models;
using gamexDesktopApp.State.Accounts;
using gamexDesktopApp.ViewModels;
using gamexModels;
using gamexServices;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace gamexDesktopApp.Commands
{
    public class GetWalletCommand : AsyncCommandBase
    {
        private WalletViewModel _walletViewModel;
        private readonly IUserService _userService;
        private readonly IAccountStore _accountStore;

        public GetWalletCommand(WalletViewModel walletViewModel, IUserService userService, IAccountStore accountStore)
        {
            _walletViewModel = walletViewModel;
            _userService = userService;
            _accountStore = accountStore;
        }

        public override async Task ExecuteAsync(object parameter)
        {
            try
            {
                var token = _accountStore.CurrentAccount.Token;
                var userId = GetUserIdFromJwt(token);
                var currentUser = await _userService.Get(token, userId);
                AssignValues(currentUser);
            }
            catch (Exception)
            {
                _walletViewModel.ErrorMessage = "Something goes wrong.";
            }
        }

        private int GetUserIdFromJwt(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = handler.ReadJwtToken(token);

            var id = jwtSecurityToken.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;

            return int.Parse(id);
        }

        private void AssignValues(UserDto userDto)
        {
            _accountStore.CurrentAccount.Login = userDto.Login;
            _accountStore.CurrentAccount.Email = userDto.Email;
            _accountStore.CurrentAccount.Total = userDto.Total;
            _accountStore.CurrentAccount.Role = (Role)userDto.RoleId;
            AssignValuesToViewModel();
        }

        private void AssignValuesToViewModel()
        {
            _walletViewModel.Login = _accountStore.CurrentAccount.Login;
            _walletViewModel.Email = _accountStore.CurrentAccount.Email;
            _walletViewModel.Total = _accountStore.CurrentAccount.Total;
        }
    }
}