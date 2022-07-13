using AutoMapper;
using gamexAPI.Authorization;
using gamexAPI.Entities;
using gamexAPI.Excepitons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using gamexModels;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using gamexAPI.Models;
using System.Linq.Expressions;

namespace gamexAPI.Services;

public interface IUserService
{
    void RegisterUser(RegisterUserDto dto);

    GetAllResult<UserDto> GetAll(GetAllQuery query);

    UserDto Get(int id);

    void Delete(int id);

    void ChangePassword(int id, UserPasswordDto dto);

    void Update(int id, UpdateUserDto dto);

    string GenerateJwt(LoginDto dto);
}

public class UserService : IUserService
{
    private readonly GamexDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly ILogger<UserService> _logger;
    private readonly IPasswordHasher<User> _passwordHasher;
    private readonly AuthenticationSettings _authenticationSettings;
    private readonly IAuthorizationService _authorizationService;
    private readonly IUserContextService _userContextService;

    public UserService(
        GamexDbContext dbContext,
        IMapper mapper, ILogger<UserService> logger,
        IPasswordHasher<User> passwordHasher,
        AuthenticationSettings authenticationSettings,
        IAuthorizationService authorizationService,
        IUserContextService userContextService)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _logger = logger;
        _passwordHasher = passwordHasher;
        _authenticationSettings = authenticationSettings;
        _authorizationService = authorizationService;
        _userContextService = userContextService;
    }

    public void RegisterUser(RegisterUserDto dto)
    {
        _logger.LogInformation($"User POST action invoked - Register User");

        var newUser = CreateUser(dto);

        _dbContext.Users.Add(newUser);
        _dbContext.SaveChanges();
    }

    public GetAllResult<UserDto> GetAll(GetAllQuery query)
    {
        _logger.LogInformation($"Users GET action invoked");

        var baseQuery = _dbContext
            .Users
            .Where(u => query.SearchPhrase == null || (u.Login.ToLower().Contains(query.SearchPhrase.ToLower())
                                                || u.Email.ToLower().Contains(query.SearchPhrase.ToLower())));

        if (!string.IsNullOrEmpty(query.SortBy))
        {
            var columnsSelectors = new Dictionary<string, Expression<Func<User, object>>>
            {
                { nameof(User.Login), u=>u.Login },
                { nameof(User.Email), u=>u.Email },
                { nameof(User.Total), u=>u.Total },
                { nameof(User.Role.Name), u=>u.Role.Name }
            };

            var selectedColumn = columnsSelectors[query.SortBy];

            baseQuery = query.SortDirection == SortDirection.ASC ?
                    baseQuery.OrderBy(selectedColumn)
                    : baseQuery.OrderByDescending(selectedColumn);
        }

        var users = baseQuery
            .Skip(query.PageSize * (query.PageNumber - 1))
            .Take(query.PageSize)
            .ToList();

        var totaItemsCount = baseQuery.Count();

        var usersDtos = _mapper.Map<List<UserDto>>(users);

        var result = new GetAllResult<UserDto>(usersDtos, totaItemsCount, query.PageSize, query.PageNumber);

        return result;
    }

    public UserDto Get(int id)
    {
        _logger.LogInformation($"User with id: {id} GET action invoked");

        var user = GetUserById(id);

        var userDto = _mapper.Map<UserDto>(user);

        return userDto;
    }

    public void Delete(int id)
    {
        _logger.LogInformation($"User with id: {id} DELETE action invoked");

        var user = GetUserById(id);

        //TODO dodać autoryzacje roli ADMIN
        /*
        var authorizationResult = _authorizationService.AuthorizeAsync(_userContextService.User, user,
            new ResourceOperationRequirement(ResourceOperation.Delete)).Result;

        if (!authorizationResult.Succeeded)
            throw new ForbidException();
        */

        _dbContext.Remove(user);
        _dbContext.SaveChanges();
    }

    public void ChangePassword(int id, UserPasswordDto dto)
    {
        _logger.LogInformation($"User with id: {id} PUT action invoked - ChangePassword");

        var user = GetUserById(id);

        //TODO dodać autoryzacje roli ADMIN
        /*
        var authorizationResult = _authorizationService.AuthorizeAsync(_userContextService.User, user,
            new ResourceOperationRequirement(ResourceOperation.Update)).Result;

        if (!authorizationResult.Succeeded)
            throw new ForbidException();
        */

        user.PasswordHash = _passwordHasher.HashPassword(user, dto.Password);

        _dbContext.SaveChanges();
    }

    public void Update(int id, UpdateUserDto dto)
    {
        _logger.LogInformation($"User with id: {id} PUT action invoked - UpdateUser");

        var user = GetUserById(id);

        //TODO dodać autoryzacje roli ADMIN
        /*
        var authorizationResult = _authorizationService.AuthorizeAsync(_userContextService.User, user,
           new ResourceOperationRequirement(ResourceOperation.Update)).Result;

        if (!authorizationResult.Succeeded)
            throw new ForbidException();
        */

        UpdateRecord(user, dto);

        _dbContext.SaveChanges();
    }

    public string GenerateJwt(LoginDto dto)
    {
        var user = _dbContext
            .Users
            .Include(u => u.Role)
            .FirstOrDefault(u => u.Login == dto.Login);

        if (user is null)
            throw new BadRequestException("Invalid login or password");

        var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);

        if (result == PasswordVerificationResult.Failed)
            throw new BadRequestException("Invalid login or password");

        var token = CreateToken(user);

        var tokenHandler = new JwtSecurityTokenHandler();

        return tokenHandler.WriteToken(token);
    }

    private JwtSecurityToken CreateToken(User user) =>
        new(_authenticationSettings.JwtIssuer,
            _authenticationSettings.JwtIssuer,
            CreateClaims(user),
            expires: CreateExpires(),
            signingCredentials: CreateCredentials(CreateKey()));

    private DateTime CreateExpires() =>
        DateTime.Now.AddDays(_authenticationSettings.JwtExpireDays);

    private SigningCredentials CreateCredentials(SymmetricSecurityKey key) =>
        new(key, SecurityAlgorithms.HmacSha256);

    private SymmetricSecurityKey CreateKey() =>
        new(Encoding.UTF8.GetBytes(_authenticationSettings.JwtKey));

    private List<Claim> CreateClaims(User user) =>
        new()
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim("Total", user.Total.ToString()),
            new Claim(ClaimTypes.Role, $"{user.Role.Name}")
        };

    private User GetUserById(int id)
    {
        var user = _dbContext
                .Users
                .FirstOrDefault(x => x.Id == id);

        if (user is null)
            throw new NotFoundException("User not found");

        return user;
    }

    private void UpdateRecord(User user, UpdateUserDto dto)
    {
        if (dto.Login is not null)
            user.Login = dto.Login;

        if (dto.Password is not null)
            user.PasswordHash = _passwordHasher.HashPassword(user, dto.Password);

        if (dto.Email is not null)
            user.Email = dto.Email;

        if (dto.Total is not null)
            user.Total = (decimal)dto.Total;

        if (dto.RoleId is not null)
            user.RoleId = (int)dto.RoleId;
    }

    private User CreateUser(RegisterUserDto dto)
    {
        decimal tmpTotal = 0;

        if (dto.Total is not null)
            tmpTotal = (decimal)dto.Total;

        var resultUser = new User()
        {
            Login = dto.Login,
            Email = dto.Email,
            Total = tmpTotal,
            RoleId = dto.RoleId
        };

        var hashedPassword = _passwordHasher.HashPassword(resultUser, dto.Password);
        resultUser.PasswordHash = hashedPassword;

        return resultUser;
    }
}