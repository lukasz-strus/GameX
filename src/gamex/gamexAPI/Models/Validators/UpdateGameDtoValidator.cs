using FluentValidation;
using gamexAPI.Entities;
using gamexModels;

namespace gamexAPI.Models.Validators;

public class UpdateGameDtoValidator : AbstractValidator<UpdateGameDto>
{
    public UpdateGameDtoValidator(GamexDbContext dbContext)
    {
        RuleFor(x => x.Name)
            .MaximumLength(100);

        RuleFor(x => x.Name)
            .Custom((value, context) =>
            {
                var emialInUse = dbContext.Games.Any(u => u.Name == value);
                if (emialInUse)
                {
                    context.AddFailure("Game", "That name of game is taken");
                }
            });

        RuleFor(x => x.Description)
            .MaximumLength(300);
    }
}