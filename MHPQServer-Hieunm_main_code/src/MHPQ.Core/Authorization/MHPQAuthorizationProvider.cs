using Abp.Authorization;
using Abp.Localization;
using Abp.MultiTenancy;

namespace MHPQ.Authorization
{
    public class MHPQAuthorizationProvider : AuthorizationProvider
    {
        public override void SetPermissions(IPermissionDefinitionContext context)
        {
            context.CreatePermission(PermissionNames.Pages_Users, L("Users"));
            context.CreatePermission(PermissionNames.Pages_Users_Activation, L("UsersActivation"));
            context.CreatePermission(PermissionNames.Pages_Roles, L("Roles"));
            context.CreatePermission(PermissionNames.Pages_User_Detail, L("UserDetail"));
            context.CreatePermission(PermissionNames.Pages_Smarthome, L("Smarthome"));
            context.CreatePermission(PermissionNames.Pages_SmartSocial, L("SmartSocial"));
            context.CreatePermission(PermissionNames.Pages_SmartCommunity, L("SmartCommunity"));
            context.CreatePermission(PermissionNames.Pages_SmartCommunity_Citizen, L("CitizenManager"));
            context.CreatePermission(PermissionNames.Pages_Tenants, L("Tenants"), multiTenancySides: MultiTenancySides.Host);
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, MHPQConsts.LocalizationSourceName);
        }
    }
}
