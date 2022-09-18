using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gamexModels.Tests.Validators;

public class UserPasswordDtoValidatorTests : ValidatorsBaseTest
{
    [Theory]
    [MemberData(nameof(ValidatorTestsData.GetSampleValidPasswordData), MemberType = typeof(ValidatorTestsData))]
    public void Validate_ForValidModel_ReturnsSuccess(UserPasswordDto model)
    {
        var validator = new UserPasswordDtoValidator(_dbContext);

        var result = validator.TestValidate(model);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Theory]
    [MemberData(nameof(ValidatorTestsData.GetSampleInvalidPasswordData), MemberType = typeof(ValidatorTestsData))]
    public void Validate_ForInvalidModel_ReturnsFailure(UserPasswordDto model)
    {
        var validator = new UserPasswordDtoValidator(_dbContext);

        var result = validator.TestValidate(model);

        result.ShouldHaveAnyValidationError();
    }

    protected override void Seed()
    {
    }
}