namespace gamexModels.Tests.Validators;

public class CreateGameDtoValidatorTests : ValidatorsBaseTest
{
    [Theory]
    [MemberData(nameof(ValidatorTestsData.GetSampleValidCreateGameData), MemberType = typeof(ValidatorTestsData))]
    public void Validate_ForValidModel_ReturnsSuccess(CreateGameDto model)
    {
        var validator = new CreateGameDtoValidator(_dbContext);

        var result = validator.TestValidate(model);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Theory]
    [MemberData(nameof(ValidatorTestsData.GetSampleInvalidCreateGameData), MemberType = typeof(ValidatorTestsData))]
    public void Validate_ForInvalidModel_ReturnsFailure(CreateGameDto model)
    {
        var validator = new CreateGameDtoValidator(_dbContext);

        var result = validator.TestValidate(model);

        result.ShouldHaveAnyValidationError();
    }

    protected override void Seed()
    {
        var testGames = new List<Game>()
        {
            new Game()
            {
                Name = "test1",
                Description = "test1",
                Price = 100m
            },
            new Game()
            {
                Name = "test2",
                Description = "test2",
                Price = 100m
            }
        };

        _dbContext.Games.AddRange(testGames);
        _dbContext.SaveChanges();
    }
}