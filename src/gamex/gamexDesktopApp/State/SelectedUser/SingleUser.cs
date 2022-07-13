using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gamexDesktopApp.State.SelectedUser;

public interface ISingleUser
{
    int? Id { get; set; }
}

public class SingleUser : ISingleUser
{
    public int? Id { get; set; }
}