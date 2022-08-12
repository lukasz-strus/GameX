using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gamexDesktopApp.State.Selected;

public interface ISingleUser : ISelected
{
}

public class SingleUser : ISingleUser
{
    public int? Id { get; set; }
}