using Abp.AspNetCore;
using Abp.AspNetCore.TestBase;
using Abp.Modules;
using Abp.Reflection.Extensions;
using MHPQ.EntityFrameworkCore;
using MHPQ.Web.Startup;
using Microsoft.AspNetCore.Mvc.ApplicationParts;

namespace MHPQ.Web.Tests
{
    [DependsOn(
        typeof(MHPQWebMvcModule),
        typeof(AbpAspNetCoreTestBaseModule)
    )]
    public class MHPQWebTestModule : AbpModule
    {
        public MHPQWebTestModule(MHPQEntityFrameworkModule abpProjectNameEntityFrameworkModule)
        {
            abpProjectNameEntityFrameworkModule.SkipDbContextRegistration = true;
        } 
        
        public override void PreInitialize()
        {
            Configuration.UnitOfWork.IsTransactional = false; //EF Core InMemory DB does not support transactions.
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(MHPQWebTestModule).GetAssembly());
        }
        
        public override void PostInitialize()
        {
            IocManager.Resolve<ApplicationPartManager>()
                .AddApplicationPartsIfNotAddedBefore(typeof(MHPQWebMvcModule).Assembly);
        }
    }
}