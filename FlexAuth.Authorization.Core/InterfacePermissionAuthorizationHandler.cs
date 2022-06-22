using FlexAuth.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace FlexAuth.Authorization.Core
{
    public partial class InterfacePermissionAuthorizationHandler<T> : AuthorizationHandler<GenericAuthorizationRequirement<T>> where T: System.Enum 
    {
        private IRequestRoleProvider _RequestContextRoles;
        private IGenericPermissionRoleProvider<T> _applicationRoleProvider;

        public InterfacePermissionAuthorizationHandler(IRequestRoleProvider rc, IGenericPermissionRoleProvider<T> permissionRoleProvider)
        {
            _RequestContextRoles = rc;
            _applicationRoleProvider = permissionRoleProvider;
        }
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, GenericAuthorizationRequirement<T> requirement)
        {
            var userRoles = _applicationRoleProvider.GetRoles().Where(r =>_RequestContextRoles.GetRoles().Contains(r.Name)).ToList();

            var userPermissions = default(T);

            
            foreach (var role in userRoles)
            {
                //userPermissions = role.Permissions;
                if(role.Permissions.HasFlag(requirement.Permissions))
                //if(requirement.Permissions.(role.Permissions))
                {
                    context.Succeed(requirement);
                    return Task.CompletedTask;
                }
            }
               // userPermissions |= role.Permissions;

            //var permissionsValue = (int)userPermissions;


            //if ((userPermissions & requirement.Permissions) != 0)
            //{
            //    context.Succeed(requirement);
            //    return Task.CompletedTask;
            //}

            return Task.CompletedTask;
        }
        
        //protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, GenericAuthorizationRequirement<T> requirement)
        //{
        //    var userRoles = _applicationRoleProvider.GetRoles().Where(r =>
        //        _RequestContextRoles.GetRoles().Contains(r.Name)).ToList();

        //    var userPermissions = default(T);

        //    foreach (var role in userRoles)

        //        userPermissions |= role.Permissions;

        //    var permissionsValue = (int)userPermissions;


        //    if ((userPermissions & requirement.Permissions) != 0)
        //    {
        //        context.Succeed(requirement);
        //        return Task.CompletedTask;
        //    }

        //    return Task.CompletedTask;
        //}
    }
}

