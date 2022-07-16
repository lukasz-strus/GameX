using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gamexAPI.Controllers;

[Route("file")]
[Authorize]
public class FileController : ControllerBase
{
    [HttpGet]
    public ActionResult GetGameImage([FromQuery] int gameId)
    {
        var rootPath = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
        var pathToSearch = $"{rootPath}/PrivateFiles/Games/";

        string partialName = gameId.ToString();
        string fullName;

        DirectoryInfo hdDirectoryInWhichToSearch = new DirectoryInfo(pathToSearch);
        FileInfo[] filesInDir = hdDirectoryInWhichToSearch.GetFiles(partialName + "*.*");

        fullName = filesInDir.FirstOrDefault().FullName;

        if (fullName is null)
        {
            return NotFound();
        }

        var extension = Path.GetExtension(fullName);

        return File(fullName, extension);
    }
}