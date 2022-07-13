using FluentValidation;
using gamexAPI.Entities;
using gamexModels;

namespace gamexAPI.Models.Validators;

public class UserPasswordDtoValidator : AbstractValidator<UserPasswordDto>
{
    public UserPasswordDtoValidator(GamexDbContext dbContext)
    {
        RuleFor(x => x.Password)
            .MinimumLength(8);

        RuleFor(x => x.ConfirmPassword).Equal(e => e.Password);
    }
}