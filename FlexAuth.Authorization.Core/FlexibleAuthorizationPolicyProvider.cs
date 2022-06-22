using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace FlexAuth.Authorization.Core;

public class FlexibleAuthorizationPolicyProvider<T> : DefaultAuthorizationPolicyProvider where T :struct, System.Enum 
{
    private readonly AuthorizationOptions _options;
    private string _policyPrefix { get; set; }
    public FlexibleAuthorizationPolicyProvider(IOptions<AuthorizationOptions> options)
        : base(options)
    {
        _options = options.Value;
        _policyPrefix  = typeof(T).Name;
    }

    public override async Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
    {
        var policy = await base.GetPolicyAsync(policyName);

        if (policy == null && PolicyNameHelper<T>.IsValidPolicyName(policyName,_policyPrefix))
        {
            var permissions = PolicyNameHelper<T>.GetPermissionsFrom(policyName,_policyPrefix);

            policy = new AuthorizationPolicyBuilder()
                .AddRequirements(new GenericAuthorizationRequirement<T>(permissions))
                .Build();

            _options.AddPolicy(policyName!, policy);
        }

        return policy;
    }
}


