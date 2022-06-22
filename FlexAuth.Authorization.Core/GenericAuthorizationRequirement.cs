using Microsoft.AspNetCore.Authorization;

namespace FlexAuth.Authorization.Core;

public class GenericAuthorizationRequirement<T> : IAuthorizationRequirement where T : System.Enum 
{
    public GenericAuthorizationRequirement(T permission)
    {
        Permissions = permission;
    }

    public T Permissions { get; }
}
