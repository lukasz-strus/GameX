namespace gamexModels.Tests.Validators;

public class UpdateGameDtoValidatorTests : ValidatorsBaseTest
{
    [Theory]
    [MemberData(nameof(ValidatorTestsData.GetSampleValidUpdateGameData), MemberType = typeof(ValidatorTestsData))]
    public void Validate_ForValidModel_ReturnsSuccess(UpdateGameDto model)
    {
        var validator = new UpdateGameDtoValidator(_dbContext);

        var result = validator.TestValidate(model);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Theory]
    [MemberData(nameof(ValidatorTestsData.GetSampleInvalidUpdateGameData), MemberType = typeof(ValidatorTestsData))]
    public void Validate_ForInvalidModel_ReturnsFailure(UpdateGameDto model)
    {
        var validator = new UpdateGameDtoValidator(_dbContext);

        var result = validator.TestValidate(model);

        result.ShouldHaveAnyValidationError();
    }

    protected override void Seed()
    {
        var testGames = new List<Game>()
        {
            new Game()
            {
                Name = "test5",
                Description = "test5",
                Price = 100m
            }
        };

        _dbContext.Games.AddRange(testGames);
        _dbContext.SaveChanges();
    }
}