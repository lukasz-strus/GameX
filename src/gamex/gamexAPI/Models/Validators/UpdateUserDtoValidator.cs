using FluentValidation;
using gamexAPI.Entities;
using gamexModels;

namespace gamexAPI.Models.Validators;

public class UpdateUserDtoValidator : AbstractValidator<UpdateUserDto>
{
    public UpdateUserDtoValidator(GamexDbContext dbContext)
    {
        RuleFor(x => x.Login)
            .MaximumLength(100);

        RuleFor(x => x.Login)
            .Custom((value, context) =>
            {
                var emialInUse = dbContext.Users.Any(u => u.Login == value);
                if (emialInUse)
                {
                    context.AddFailure("Login", "That login is taken");
                }
            });

        RuleFor(x => x.Password)
            .MinimumLength(8);

        RuleFor(x => x.ConfirmPassword).Equal(e => e.Password);

        RuleFor(x => x.Email)
            .MaximumLength(100)
            .EmailAddress();

        RuleFor(x => x.ConfirmEmail).Equal(e => e.Email);

        RuleFor(x => x.Email)
            .Custom((value, context) =>
            {
                var emialInUse = dbContext.Users.Any(u => u.Email == value);
                if (emialInUse)
                {
                    context.AddFailure("Email", "That email is taken");
                }
            });
    }
}