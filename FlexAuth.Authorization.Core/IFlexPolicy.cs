using Microsoft.AspNetCore.Authorization;

namespace FlexAuth.Authorization.Core;

public interface IFlexPolicy
{
    Task<AuthorizationPolicy?> TryGetPolicyAsync(string policyName);
    string GetPolicyPrefix();
}

public class GenericFlexPolicy<T> : IFlexPolicy where T : struct, System.Enum
{
    public virtual string GetPolicyPrefix()
    {
        return typeof(T).Name;
    }

    public async Task<AuthorizationPolicy?> TryGetPolicyAsync(string policyName)
    {
        string _policyPrefix = GetPolicyPrefix();

        if (PolicyNameHelper<T>.IsValidPolicyName(policyName, _policyPrefix))
        {
            var permissions = PolicyNameHelper<T>.GetPermissionsFrom(policyName, _policyPrefix);

            var policy = new AuthorizationPolicyBuilder()
                .AddRequirements(new GenericAuthorizationRequirement<T>(permissions))
                .Build();

            return policy;
        }

        return null;
    }
}


