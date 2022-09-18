using FluentValidation.TestHelper;
using gamexEntities;
using gamexModels.Validators;
using Microsoft.EntityFrameworkCore;

namespace gamexModels.Tests;

public class RegisterUserDtoValidatorTests
{
    private GamexDbContext _dbContext;

    public RegisterUserDtoValidatorTests()
    {
        var builder = new DbContextOptionsBuilder<GamexDbContext>();
        builder.UseInMemoryDatabase("TestDb");

        _dbContext = new GamexDbContext(builder.Options);
        Seed();
    }

    protected void Seed()
    {
        var testUsers = new List<User>()
            {
                new User()
                {
                    Login = "test1",
                    Email = "test1@test.com",
                    PasswordHash = "password123"
                },
                 new User()
                {
                    Login = "test2",
                    Email = "test2@test.com",
                    PasswordHash = "password123"
                },
            };

        _dbContext.Users.AddRange(testUsers);
        _dbContext.SaveChanges();
    }

    [Fact]
    public void Validate_ForValidModel_ReturnsSuccess()
    {
        var model = new RegisterUserDto()
        {
            Login = "Test123",
            Email = "test123@test.com",
            ConfirmEmail = "test123@test.com",
            Password = "password123",
            ConfirmPassword = "password123"
        };

        var validator = new RegisterUserDtoValidator(_dbContext);

        var result = validator.TestValidate(model);

        result.ShouldNotHaveAnyValidationErrors();
    }

    //TODO przerobić na THEORY
    [Fact]
    public void Validate_ForInvalidModel_ReturnsFailure()
    {
        var model = new RegisterUserDto()
        {
            Login = "test1",
            Email = "test123@test.com",
            ConfirmEmail = "test123@test.com",
            Password = "password123",
            ConfirmPassword = "password123"
        };

        var validator = new RegisterUserDtoValidator(_dbContext);

        var result = validator.TestValidate(model);

        result.ShouldHaveAnyValidationError();
    }
}