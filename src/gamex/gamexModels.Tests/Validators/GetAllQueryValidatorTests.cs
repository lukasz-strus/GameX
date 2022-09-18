namespace gamexModels.Tests.Validators;

public class GetAllQueryValidatorTests
{
    [Theory]
    [MemberData(nameof(ValidatorTestsData.GetSampleValidData), MemberType = typeof(ValidatorTestsData))]
    public void Validate_ForCorrectModel_ReturnsSuccess(GetAllQuery model)
    {
        var validator = new GetAllQueryValidator();

        var result = validator.TestValidate(model);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Theory]
    [MemberData(nameof(ValidatorTestsData.GetSampleInvalidData), MemberType = typeof(ValidatorTestsData))]
    public void Validate_ForIncorrectModel_ReturnsFailure(GetAllQuery model)
    {
        var validator = new GetAllQueryValidator();

        var result = validator.TestValidate(model);

        result.ShouldHaveAnyValidationError();
    }
}