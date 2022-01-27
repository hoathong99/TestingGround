using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Abp.Modules;
using Abp.Reflection.Extensions;
using MHPQ.Configuration;

namespace MHPQ.Web.Host.Startup
{
    [DependsOn(
       typeof(MHPQWebCoreModule))]
    public class MHPQWebHostModule: AbpModule
    {
        private readonly IWebHostEnvironment _env;
        private readonly IConfigurationRoot _appConfiguration;

        public MHPQWebHostModule(IWebHostEnvironment env)
        {
            _env = env;
            _appConfiguration = env.GetAppConfiguration();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(MHPQWebHostModule).GetAssembly());
        }
    }
}
