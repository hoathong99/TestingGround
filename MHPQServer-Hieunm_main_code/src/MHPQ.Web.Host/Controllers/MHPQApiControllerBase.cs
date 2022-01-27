using Abp.WebApi.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MHPQ.Web.Host.Controllers
{
    public class MHPQApiControllerBase: AbpApiController
    {
        public MHPQApiControllerBase()
        {
            LocalizationSourceName = MHPQConsts.LocalizationSourceName;
        }
    }
}
