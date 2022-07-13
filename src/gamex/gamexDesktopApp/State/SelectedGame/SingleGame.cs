using gamexDesktopApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gamexDesktopApp.State.SelectedGame;

public interface ISingleGame
{
    int? Id { get; set; }
}

public class SingleGame : ISingleGame
{
    public int? Id { get; set; }
}