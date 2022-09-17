namespace gamexAPI.Controllers;

[Route("file")]
[Authorize]
public class FileController : ControllerBase
{
    private readonly IFileService _fileService;

    public FileController(IFileService fileService)
    {
        _fileService = fileService;
    }

    [HttpGet("/all/{gameId}")]
    public ActionResult<IEnumerable<Image>> GetGameImages([FromRoute] int gameId)
    {
        var images = _fileService.GetImages(gameId);

        return Ok(images);
    }

    [HttpGet("{gameId}")]
    public ActionResult<Image> Get([FromRoute] int gameId)
    {
        var image = _fileService.GetMainImage(gameId);

        return Ok(image);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin,Seller")]
    public ActionResult Delete([FromRoute] int id)
    {
        _fileService.Delete(id);

        return NoContent();
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Seller")]
    public ActionResult Create([FromBody] CreateImageDto dto)
    {
        var id = _fileService.Create(dto);

        return Created($"/file/{id}", null);
    }
}