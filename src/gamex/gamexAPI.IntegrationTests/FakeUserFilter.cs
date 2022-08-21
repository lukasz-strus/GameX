using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace gamexAPI.IntegrationTests;

public class FakeUserFilter : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var claimsPrincipal = new ClaimsPrincipal();

        claimsPrincipal.AddIdentity(new ClaimsIdentity(
            new[]
            {
                new Claim(ClaimTypes.Role, "Admin")
            }));

        context.HttpContext.User = claimsPrincipal;

        await next();
    }
}