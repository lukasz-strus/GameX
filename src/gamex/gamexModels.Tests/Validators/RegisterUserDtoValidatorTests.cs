namespace gamexModels.Tests.Validators;

public class RegisterUserDtoValidatorTests : ValidatorsBaseTest
{
    [Theory]
    [MemberData(nameof(ValidatorTestsData.GetSampleValidRegisterData), MemberType = typeof(ValidatorTestsData))]
    public void Validate_ForValidModel_ReturnsSuccess(RegisterUserDto model)
    {
        var validator = new RegisterUserDtoValidator(_dbContext);

        var result = validator.TestValidate(model);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Theory]
    [MemberData(nameof(ValidatorTestsData.GetSampleInvalidRegisterData), MemberType = typeof(ValidatorTestsData))]
    public void Validate_ForInvalidModel_ReturnsFailure(RegisterUserDto model)
    {
        var validator = new RegisterUserDtoValidator(_dbContext);

        var result = validator.TestValidate(model);

        result.ShouldHaveAnyValidationError();
    }

    protected override void Seed()
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
}