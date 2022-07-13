using FluentValidation;
using gamexAPI.Entities;

namespace gamexAPI.Models.Validators;

public class GetAllQueryValidator : AbstractValidator<GetAllQuery>
{
    private readonly int[] allowedPageSizes = new[] { 5, 10, 15 };

    private readonly string[] allowedSortByColumnNames =
    {
        nameof(User.Login),
        nameof(User.Email),
        nameof(User.Total),
        nameof(User.Role.Name),
        nameof(Game.Name),
        nameof(Game.Description),
        nameof(Game.Price)
    };

    public GetAllQueryValidator()
    {
        RuleFor(r => r.PageNumber).GreaterThanOrEqualTo(1);
        RuleFor(r => r.PageSize).Custom((value, context) =>
        {
            if (!allowedPageSizes.Contains(value))
            {
                context.AddFailure("PageSize", $"PageSize must in [{string.Join(",", allowedPageSizes)}]");
            }
        });

        RuleFor(r => r.SortBy).Must(value => string.IsNullOrEmpty(value) || allowedSortByColumnNames.Contains(value))
            .WithMessage($"Sort by is optional, or must by in [{string.Join(",", allowedSortByColumnNames)}]");
    }
}