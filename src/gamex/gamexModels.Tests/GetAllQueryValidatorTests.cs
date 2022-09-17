using FluentValidation.TestHelper;
using gamexEntities;
using gamexModels.Validators;

namespace gamexModels.Tests;

public class GetAllQueryValidatorTests
{
    public static IEnumerable<object[]> GetSampleValidData()
    {
        var list = new List<GetAllQuery>()
        {
            new GetAllQuery()
            {
                PageNumber = 1,
                PageSize = 5
            },
            new GetAllQuery()
            {
                PageNumber = 2,
                PageSize = 10
            },
            new GetAllQuery()
            {
                PageNumber = 22,
                PageSize = 15
            },
            new GetAllQuery()
            {
                PageNumber = 22,
                PageSize = 5,
                SortBy = nameof(Game.Name)
            },
            new GetAllQuery()
            {
                PageNumber = 22,
                PageSize = 15,
                SortBy = nameof(Game.Price)
            },
        };

        return list.Select(q => new object[] { q });
    }

    public static IEnumerable<object[]> GetSampleInvalidData()
    {
        var list = new List<GetAllQuery>()
        {
            new GetAllQuery()
            {
                PageNumber = 0,
                PageSize = 5
            },
            new GetAllQuery()
            {
                PageNumber = 2,
                PageSize = 13
            },
            new GetAllQuery()
            {
                PageNumber = 22,
                PageSize = 5,
                SortBy = nameof(User.PasswordHash)
            }
        };

        return list.Select(q => new object[] { q });
    }

    [Theory]
    [MemberData(nameof(GetSampleValidData))]
    public void Validate_ForCorrectModel_ReturnsSuccess(GetAllQuery model)
    {
        var validator = new GetAllQueryValidator();

        var result = validator.TestValidate(model);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Theory]
    [MemberData(nameof(GetSampleInvalidData))]
    public void Validate_ForIncorrectModel_ReturnsFailure(GetAllQuery model)
    {
        var validator = new GetAllQueryValidator();

        var result = validator.TestValidate(model);

        result.ShouldHaveAnyValidationError();
    }
}