using FluentValidation;
using FluentValidation.AspNetCore;
using gamexAPI;
using gamexAPI.Authorization;
using gamexAPI.Entities;
using gamexAPI.Middleware;
using gamexAPI.Models.Validators;
using gamexAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using gamexModels;
using NLog.Web;
using System.Reflection;
using System.Text;
using gamexAPI.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder();

// NLog: Setup NLog for Dependency injection
builder.Logging.ClearProviders();
builder.Logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
builder.Host.UseNLog();

//configure service
var authenticationSettings = new AuthenticationSettings();
builder.Configuration.GetSection("Authentication").Bind(authenticationSettings);

builder.Services.AddSingleton(authenticationSettings);
builder.Services.AddAuthentication(option =>
{
    option.DefaultAuthenticateScheme = "Bearer";
    option.DefaultScheme = "Bearer";
    option.DefaultChallengeScheme = "Bearer";
}).AddJwtBearer(cfg =>
{
    cfg.RequireHttpsMetadata = false;
    cfg.SaveToken = true;
    cfg.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = authenticationSettings.JwtIssuer,
        ValidAudience = authenticationSettings.JwtIssuer,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authenticationSettings.JwtKey))
    };
});

builder.Services.AddScoped<IAuthorizationHandler, ResourceOperationRequirementHandler>();

builder.Services.AddControllers().AddFluentValidation();

builder.Services.AddDbContext<GamexDbContext>
    (options => options.UseSqlServer(builder.Configuration.GetConnectionString("gamexDbConnection")));

builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

builder.Services.AddScoped<GamexSeeder>();

builder.Services.AddScoped<IGameService, GameService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserContextService, UserContextService>();

builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

builder.Services.AddScoped<IValidator<RegisterUserDto>, RegisterUserDtoValidator>();
builder.Services.AddScoped<IValidator<UpdateUserDto>, UpdateUserDtoValidator>();
builder.Services.AddScoped<IValidator<UserPasswordDto>, UserPasswordDtoValidator>();
builder.Services.AddScoped<IValidator<CreateGameDto>, CreateGameDtoValidator>();
builder.Services.AddScoped<IValidator<UpdateGameDto>, UpdateGameDtoValidator>();
builder.Services.AddScoped<IValidator<GetAllQuery>, GetAllQueryValidator>();

builder.Services.AddScoped<ErrorHandlingMiddleware>();
builder.Services.AddScoped<RequestTimeMiddleware>();

builder.Services.AddHttpContextAccessor();

builder.Services.AddSwaggerGen();

//builder.Services.AddCors(options =>
//{
//    options.AddPolicy("FrontEndClient", policyBuilder =>
//    policyBuilder.AllowAnyMethod()
//        .AllowAnyHeader()
//        .WithOrigins(builder.Configuration["AllowedOrigins"])
//        );
//});

// configure
var app = builder.Build();

var scope = app.Services.CreateScope();
var seeder = scope.ServiceProvider.GetRequiredService<GamexSeeder>();

app.UseStaticFiles();

//app.UseCors("FrontEndClient");

seeder.Seed();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseMiddleware<RequestTimeMiddleware>();

app.UseAuthentication();

app.UseHttpsRedirection();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "gamex API");
});

app.UseRouting();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();

/*Next stages:
 *TODO Validators in gamexAPI
 *
 *
 */