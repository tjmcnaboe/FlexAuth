# FlexAuth

Flex Auth is a library designed to decouple your authorization code from the underlying claims and create strongly typed permission policies based on user roles.   You can implement multiple permission sets each with their own corresponding policy structure that are anchored to a common set of user roles as defined by your application.  This allows you to use a simple unifying role structure while allowing underlying services to implement their own authorization/permissions structures independent of one another and of varying complexity.  These permission sets can then be applied via enum based authorization attributes that provide strongly typed access to the underlying roles/permissions.

Implement your roles and permissions

```csharp

    public class RootRoleProvider : IGenericPermissionRoleProvider<RootPermissions>//IPermissionRoleProvider<Permissions>
    {
        public List<IGenericPermissionRole<RootPermissions>> GetRoles()
        {
            var roles = new List<IGenericPermissionRole<RootPermissions>>();

            //RootPermissions globalAdminPermissions = RootPermissions.All;
            roles.Add(new RootRole(RoleConstants.GlobalAdmin, RootPermissions.All));
            
            //RootPermissions ownerPermissions = RootPermissions.Owner;
            roles.Add(new RootRole(RoleConstants.Owner, RootPermissions.Owner));

            //RootPermissions adminPermissions = RootPermissions.Admin;
            roles.Add(new RootRole(RoleConstants.Admin, RootPermissions.Admin));

            roles.Add(new RootRole(RoleConstants.AccountManager, RootPermissions.AccountManager));

            roles.Add(new RootRole(RoleConstants.Developer, RootPermissions.Dev));

            roles.Add(new RootRole(RoleConstants.Member,RootPermissions.IsMember));
            return roles;
        }


    }

    public class RootRole : IGenericPermissionRole<RootPermissions>
    {
        public RootRole(string name, RootPermissions permissions)
        {
            Name = name;
            Permissions = permissions;
        }
        public string Name { get; set; }
        public RootPermissions Permissions { get; set; }
    }

    [Flags]
    public enum RootPermissions
    {
        None = 0,
        IsAdmin = 1,
        IsDeveloper = 2,
        IsAccountManager = 4,
        IsOwner= 8,
        IsGlobalAdmin = 16,
        IsMember = 32,
        Forecast = 64,
        ViewAccessControl = 128,
        Dev = IsDeveloper + Forecast,
        AccountManager = IsAccountManager,
        Admin = IsAdmin + IsDeveloper + IsAccountManager,
        Owner = IsAdmin + IsDeveloper + IsAccountManager + IsOwner,
        All = ~None
    }

    public class RoleConstants
    {
        public const string GlobalAdmin = "globaladmin";
        public const string Owner = "owner";
        public const string Admin = "admin";
        public const string Developer = "developer";
        public const string AccountManager = "accountmanager";
        public const string Member = "member";

        public static List<string> Roles = new List<string>() { GlobalAdmin, Owner, Admin, Developer, AccountManager, Member };
    }
```


Inject your services in program/startup

```csharp
            services.AddSingleton<IFlexPolicy, RootFlexPolicy>();
            services.AddScoped<IGenericPermissionRoleProvider<RootPermissions>, RootRoleProvider>(); // provides the permissions assoscatied with a role
            services.AddScoped<IAuthorizationHandler, InterfacePermissionAuthorizationHandler<RootPermissions>>();
            services.AddSingleton<IAuthorizationPolicyProvider, MultiFlexibleAuthorizationPolicyProvider>();
            
```

Protect your controllers/actions with a policy:
```csharp
//[Authorize]
    [GenericEnumAuthorize<RootPermissions>(RootPermissions.IsDeveloper)]
    public class HomeController : Controller
    
    ```
