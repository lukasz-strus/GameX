using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gamexDesktopApp.ViewModels;

public interface IPasswordViewModel
{
    public string Password { get; set; }
    public string ConfirmPassword { get; set; }
    string ErrorMessage { set; }
    MessageViewModel ErrorMessageViewModel { get; }
}