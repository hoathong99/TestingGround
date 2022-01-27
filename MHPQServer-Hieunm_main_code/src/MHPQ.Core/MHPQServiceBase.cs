using Abp;


namespace MHPQ
{
    public abstract class MHPQServiceBase : AbpServiceBase
    {
        protected MHPQServiceBase()
        {
            LocalizationSourceName = MHPQConsts.LocalizationSourceName;
        }
    }
}
