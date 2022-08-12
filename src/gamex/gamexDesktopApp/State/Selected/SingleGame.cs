using gamexDesktopApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gamexDesktopApp.State.Selected;

public interface ISingleGame : ISelected
{
}

public class SingleGame : ISingleGame
{
    public int? Id { get; set; }
}