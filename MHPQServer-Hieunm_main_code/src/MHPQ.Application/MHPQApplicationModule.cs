using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;
using MHPQ.Authorization;

namespace MHPQ
{
    [DependsOn(
        typeof(MHPQCoreModule), 
        typeof(AbpAutoMapperModule))]
    public class MHPQApplicationModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Authorization.Providers.Add<MHPQAuthorizationProvider>();


            //Adding custom AutoMapper mappings
            Configuration.Modules.AbpAutoMapper().Configurators.Add(mapper =>
            {
                CustomDtoMapper.CreateMappings(mapper);
            });
        }

        public override void Initialize()
        {
            var thisAssembly = typeof(MHPQApplicationModule).GetAssembly();

            IocManager.RegisterAssemblyByConvention(thisAssembly);

            Configuration.Modules.AbpAutoMapper().Configurators.Add(
                // Scan the assembly for classes which inherit from AutoMapper.Profile
                cfg => cfg.AddMaps(thisAssembly)
            );

        }
    }
}
