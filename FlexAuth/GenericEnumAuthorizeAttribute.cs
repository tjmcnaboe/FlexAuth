namespace FlexAuth;

public class GenericEnumAuthorizeAttribute<T> : Microsoft.AspNetCore.Authorization.AuthorizeAttribute where T : struct, Enum
{
    public GenericEnumAuthorizeAttribute() { }

    public GenericEnumAuthorizeAttribute(string policy) : base(policy) { }

    public GenericEnumAuthorizeAttribute(T member)
    {
        _policyPrefix = typeof(T).Name;
        Permissions = member;
    }

    public string _policyPrefix { get; }

    //public abstract 
    public  T Permissions
    {
        get
        {
            return !string.IsNullOrEmpty(Policy)
                ? PolicyNameHelper<T>.GetPermissionsFrom(Policy,_policyPrefix)
                : default(T);
        }
        set
        {
            var name = PolicyNameHelper<T>.GeneratePolicyNameFor(value,_policyPrefix);
            if(name == null)
            { Policy = string.Empty; }
            else
            { Policy = name; }
        }
    }
}




