using Microsoft.AspNetCore.Authorization;

namespace gamexAPI.Authorization;

public enum ResourceOperation
{
    Create,
    Read,
    Update,
    Delete
}

public class ResourceOperationRequirement : IAuthorizationRequirement
{
    public ResourceOperation ResourceOperation { get; }

    public ResourceOperationRequirement(ResourceOperation resourceOperation)
    {
        ResourceOperation = resourceOperation;
    }
}