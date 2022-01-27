using Abp.Authorization;
using MHPQ.Authorization.Roles;
using MHPQ.Authorization.Users;

namespace MHPQ.Authorization
{
    public class PermissionChecker : PermissionChecker<Role, User>
    {
        public PermissionChecker(UserManager userManager)
            : base(userManager)
        {
        }
    }
}
