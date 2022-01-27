using Abp.AspNetCore.Mvc.Controllers;
using Abp.IdentityFramework;
using Microsoft.AspNetCore.Identity;

namespace MHPQ.Controllers
{
    public abstract class MHPQControllerBase: AbpController
    {
        protected MHPQControllerBase()
        {
            LocalizationSourceName = MHPQConsts.LocalizationSourceName;
        }

        protected void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }
    }
}
