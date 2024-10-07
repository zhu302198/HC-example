using Integration.Services.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace ExampleNamespace.GraphQL.TypeExtensions;

// Key used to reference the User object in the federated schema
[Key("userId")]
public class User
{
    public string UserId { get; set; }

    // External indicates that Profile comes from another subgraph (e.g., profile-api)
    [External]
    public Profile Profile { get; set; }

    // This field requires the 'profile { username }' selection from the external type Profile
    [Requires("profile { username }")]
    public async Task<bool> IsProfileActiveAsync(
        [Service] ILogger<User> logger)
    {
        if (string.IsNullOrEmpty(Profile?.Username))
        {
            logger.LogWarning("Username is null or empty for user.");
            return false;
        }

        return result.IsActive;
    }

    // ReferenceResolver used to resolve the User object in this subgraph
    [ReferenceResolver]
    public static User ResolveReference(string userId, Profile profile)
    {
        return new User
        {
            UserId = userId,
            Profile = profile
        };
    }
}

// External Profile type that comes from another subgraph
public class Profile
{
    public string Username { get; set; }
}

