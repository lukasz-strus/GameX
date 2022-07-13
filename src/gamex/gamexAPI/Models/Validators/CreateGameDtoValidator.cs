using FluentValidation;
using gamexAPI.Entities;
using gamexModelsDto;

namespace gamexAPI.Models.Validators;

public class CreateGameDtoValidator : AbstractValidator<CreateGameDto>
{
    public CreateGameDtoValidator(GamexDbContext dbContext)
    {
        RuleFor(x => x.Name)
            .NotEmpty()
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

        RuleFor(x => x.Price)
            .NotEmpty();
    }
}