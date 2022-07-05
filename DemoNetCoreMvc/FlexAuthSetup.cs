using FlexAuth;
using FlexAuth.Authorization.Core;
using FlexAuth.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace DemoNetCoreMvc
{
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

            roles.Add(new RootRole(RoleConstants.Member, RootPermissions.IsMember));
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
        IsOwner = 8,
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

    public class RootFlexPolicy : IFlexPolicy
    {
        public string GetPolicyPrefix()
        {
            throw new NotImplementedException();
        }

        public async Task<AuthorizationPolicy?> TryGetPolicyAsync(string policyName)
        {
            string _policyPrefix = typeof(RootPermissions).Name;

            if (PolicyNameHelper<RootPermissions>.IsValidPolicyName(policyName, _policyPrefix))
            {
                var permissions = PolicyNameHelper<RootPermissions>.GetPermissionsFrom(policyName, _policyPrefix);

                var policy = new AuthorizationPolicyBuilder()
                    .AddRequirements(new GenericAuthorizationRequirement<RootPermissions>(permissions))
                    .Build();

                return policy;
            }

            return null;
        }
    }


    public class StaticTestingRoleProvider : IRequestRoleProvider
    {
        public List<string> GetRoles()
        {
            //return new List<string>() { RoleConstants.Owner };

            return new List<string>() { RoleConstants.Developer };
        }
    }
}
