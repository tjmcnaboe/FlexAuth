using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace FlexAuth.Authorization.Core;

public class MultiFlexibleAuthorizationPolicyProvider : DefaultAuthorizationPolicyProvider //where T : struct, System.Enum
{
    private readonly AuthorizationOptions _options;
    private readonly IEnumerable<IFlexPolicy> _flexPolicytypes;

    private List<string> _policyPrefix { get; set; }
    public MultiFlexibleAuthorizationPolicyProvider(IOptions<AuthorizationOptions> options, IEnumerable<IFlexPolicy> flexPlicy)
        : base(options)
    {

        _options = options.Value;
        _flexPolicytypes = flexPlicy;
    }

    public override async Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
    {
        var policy = await base.GetPolicyAsync(policyName);
        if (policy == null)
        {
            foreach (var t in _flexPolicytypes)
            {
                if (policy == null)
                {
                    policy = await t.TryGetPolicyAsync(policyName);
                }
            }
        }

        return policy;
    }

}


