namespace gamexAPI.Services;

public interface IFileService
{
    List<ImageDto> GetImages(int gameId);

    public ImageDto GetMainImage(int gameId);

    public int Create(CreateImageDto imageDto);

    public void Delete(int id);
}

public class FileService : IFileService
{
    private readonly GamexDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly ILogger<GameService> _logger;

    public FileService(GamexDbContext dbContext,
                        IMapper mapper,
                        ILogger<GameService> logger)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _logger = logger;
    }

    public List<ImageDto> GetImages(int gameId)
    {
        _logger.LogInformation($"Images for game with id: {gameId} GET action invoked");

        var images = _dbContext
            .Images
            .Where(i => i.GameId == gameId)
            .ToList();

        if (images is null)
            throw new NotFoundException("Image not found");

        var imageDto = _mapper.Map<List<ImageDto>>(images);

        return imageDto;
    }

    public ImageDto GetMainImage(int gameId)
    {
        _logger.LogInformation($"Main image for game with id: {gameId} GET action invoked");

        var image = _dbContext
            .Images
            .OrderBy(i => i.Id)
            .FirstOrDefault(i => i.GameId == gameId);

        if (image is null)
            throw new NotFoundException("Image not found");

        var imageDto = _mapper.Map<ImageDto>(image);

        return imageDto;
    }

    public void Delete(int id)
    {
        _logger.LogInformation($"Image with id: {id} DELETE action invoked");

        var image = _dbContext
            .Images
            .FirstOrDefault(i => i.Id == id);

        if (image is null)
            throw new NotFoundException("Image not found");

        _dbContext.Images.Remove(image);
        _dbContext.SaveChanges();
    }

    public int Create(CreateImageDto imageDto)
    {
        _logger.LogInformation($"Image POST action invoked");

        var image = _mapper.Map<Image>(imageDto);

        _dbContext.Images.Add(image);
        _dbContext.SaveChanges();

        return image.Id;
    }
}