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
        var rootPath = Directory.GetCurrentDirectory();
        var pathToSearch = $"{rootPath}/PrivateFiles/Games/";

        string partialName = gameId.ToString();

        var hdDirectoryInWhichToSearch = new DirectoryInfo(pathToSearch);
        FileInfo[] filesInDir = hdDirectoryInWhichToSearch.GetFiles("*" + partialName + "*.*");

        string fileName;
        string filePath;

        if (filesInDir.Length == 0)
        {
            fileName = "0.jpg";
            filePath = Path.Combine(pathToSearch, fileName);
        }
        else
        {
            filePath = filesInDir.FirstOrDefault().FullName;

            fileName = filesInDir.FirstOrDefault().Name;
        }

        var contentProvider = new FileExtensionContentTypeProvider();
        contentProvider.TryGetContentType(fileName, out var contentType);

        var fileContents = System.IO.File.ReadAllBytes(filePath);

        return File(fileContents, contentType, fileName);
    }
}