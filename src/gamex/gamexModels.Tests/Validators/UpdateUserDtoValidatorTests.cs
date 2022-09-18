using Microsoft.EntityFrameworkCore;

namespace gamexModels.Tests.Validators;

public class UpdateUserDtoValidatorTests : ValidatorsBaseTest
{
    [Theory]
    [MemberData(nameof(ValidatorTestsData.GetSampleValidUpdateUserData), MemberType = typeof(ValidatorTestsData))]
    public void Validate_ForValidModel_ReturnsSuccess(UpdateUserDto model)
    {
        var validator = new UpdateUserDtoValidator(_dbContext);

        var result = validator.TestValidate(model);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Theory]
    [MemberData(nameof(ValidatorTestsData.GetSampleInvalidUpdateUserData), MemberType = typeof(ValidatorTestsData))]
    public void Validate_ForValidModel_ReturnsFailure(UpdateUserDto model)
    {
        var validator = new UpdateUserDtoValidator(_dbContext);

        var result = validator.TestValidate(model);

        result.ShouldHaveAnyValidationError();
    }

    protected override void Seed()
    {
        var testUsers = new List<User>()
            {
                new User()
                {
                    Login = "test5",
                    Email = "test5@test.com",
                    PasswordHash = "password123"
                },
                 new User()
                {
                    Login = "test6",
                    Email = "test6@test.com",
                    PasswordHash = "password123"
                },
            };

        _dbContext.Users.AddRange(testUsers);
        _dbContext.SaveChanges();
    }
}