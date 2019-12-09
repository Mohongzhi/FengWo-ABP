using Abp.Authorization;
using FengWo.Authorization.Roles;
using FengWo.Authorization.Users;

namespace FengWo.Authorization
{
    public class PermissionChecker : PermissionChecker<Role, User>
    {
        public PermissionChecker(UserManager userManager)
            : base(userManager)
        {
        }
    }
}
