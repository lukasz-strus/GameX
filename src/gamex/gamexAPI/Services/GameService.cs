namespace gamexAPI.Services;

public interface IGameService
{
    int Create(CreateGameDto dto);

    GetAllResult<GameDto> GetAll(GetAllQuery query);

    GameDto Get(int id);

    void Delete(int id);

    void Update(int id, UpdateGameDto dto);

    GameSerialDto GetSerialKey(int userId, int gameId);
}

public class GameService : IGameService
{
    private readonly GamexDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly ILogger<GameService> _logger;

    public GameService(GamexDbContext dbContext,
                        IMapper mapper,
                        ILogger<GameService> logger)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _logger = logger;
    }

    public int Create(CreateGameDto dto)
    {
        _logger.LogInformation($"Game POST action invoked");

        var game = _mapper.Map<Game>(dto);
        _dbContext.Games.Add(game);
        _dbContext.SaveChanges();

        return game.Id;
    }

    public GetAllResult<GameDto> GetAll(GetAllQuery query)
    {
        _logger.LogInformation($"Games GET action invoked");

        var baseQuery = _dbContext
            .Games
            .Where(g => query.SearchPhrase == null || (g.Name.ToLower().Contains(query.SearchPhrase.ToLower())
                                                || g.Description.ToLower().Contains(query.SearchPhrase.ToLower())));

        if (!string.IsNullOrEmpty(query.SortBy))
        {
            var columnsSelector = new Dictionary<string, Expression<Func<Game, object>>>
            {
                { nameof(Game.Name), g=>g.Name },
                { nameof(Game.Description), g=>g.Description },
                { nameof(Game.Price), g=>g.Price }
            };

            var selectedColumn = columnsSelector[query.SortBy];

            baseQuery = query.SortDirection == SortDirection.ASC ?
                                baseQuery.OrderBy(selectedColumn)
                                : baseQuery.OrderByDescending(selectedColumn);
        }

        var games = baseQuery
            .Skip(query.PageSize * (query.PageNumber - 1))
            .Take(query.PageSize)
            .ToList();

        var totaItemsCount = baseQuery.Count();

        var gamesDtos = _mapper.Map<List<GameDto>>(games);

        var result = new GetAllResult<GameDto>(gamesDtos, totaItemsCount, query.PageSize, query.PageNumber);

        return result;
    }

    public GameDto Get(int id)
    {
        _logger.LogInformation($"Game with id: {id} GET action invoked");

        var game = GetGameById(id);

        var gameDto = _mapper.Map<GameDto>(game);

        return gameDto;
    }

    public void Delete(int id)
    {
        _logger.LogInformation($"Game with id: {id} DELETE action invoked");

        var game = GetGameById(id);

        _dbContext.Games.Remove(game);
        _dbContext.SaveChanges();
    }

    public void Update(int id, UpdateGameDto dto)
    {
        _logger.LogInformation($"Game with id: {id} PUT action invoked");

        var game = GetGameById(id);

        UpdateGameRecord(game, dto);

        _dbContext.SaveChanges();
    }

    public GameSerialDto GetSerialKey(int userId, int gameId)
    {
        _logger.LogInformation($"User with ID {gameId} try to buy game with ID {userId}");

        var game = GetGameById(gameId);
        var user = GetUserById(userId);

        if (user.Total < game.Price)
            throw new NotEnoughFundsException("Not enough funds in the wallet");

        user.Total -= game.Price;
        _dbContext.SaveChanges();

        var gameKey = new GameSerialDto()
        {
            Value = GenerateGameKey(game)
        };

        return gameKey;
    }

    private string GenerateGameKey(Game game)
    {
        var gameSerials = _dbContext.GameSerials;

        string serialValue = CreatKey();

        var serial = CreateGameSerial(game, serialValue);

        gameSerials.Add(serial);
        _dbContext.SaveChanges();

        return serial.Value;
    }

    private GameSerial CreateGameSerial(Game game, string serialValue) =>
        new()
        {
            Value = serialValue,
            GameId = game.Id
        };

    private string CreatKey()
    {
        var gameSerials = _dbContext.GameSerials;
        string result;

        while (true)
        {
            result = Guid.NewGuid().ToString();
            var existingSerial = gameSerials.FirstOrDefault(x => x.Value == result);

            if (existingSerial is null)
                break;
        }

        return result;
    }

    private User GetUserById(int id)
    {
        var user = _dbContext
                .Users
                .FirstOrDefault(x => x.Id == id);

        if (user is null)
            throw new NotFoundException("User not found");

        return user;
    }

    private Game GetGameById(int id)
    {
        var game = _dbContext
                 .Games
                 .FirstOrDefault(x => x.Id == id);

        if (game is null)
            throw new NotFoundException("Game not found");

        return game;
    }

    private void UpdateGameRecord(Game game, UpdateGameDto dto)
    {
        if (dto.Name is not null)
            game.Name = dto.Name;

        if (dto.Description is not null)
            game.Description = dto.Description;

        if (dto.Price is not null)
            game.Price = (decimal)dto.Price;
    }
}