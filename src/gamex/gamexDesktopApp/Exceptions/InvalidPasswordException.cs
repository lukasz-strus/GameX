using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gamexDesktopApp.Exceptions
{
    public class InvalidPasswordException : Exception
    {
        public string Login { get; set; }
        public string Password { get; set; }

        public InvalidPasswordException(string login, string password)
        {
            Login = login;
            Password = password;
        }

        public InvalidPasswordException(string message, string login, string password) : base(message)
        {
            Login = login;
            Password = password;
        }

        public InvalidPasswordException(string message, Exception innerException, string login, string password) : base(message, innerException)
        {
            Login = login;
            Password = password;
        }
    }
}