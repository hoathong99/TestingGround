using Abp.Application.Services;
using MHPQ.MultiTenancy.Dto;

namespace MHPQ.MultiTenancy
{
    public interface ITenantAppService : IAsyncCrudAppService<TenantDto, int, PagedTenantResultRequestDto, CreateTenantDto, TenantDto>
    {
    }
}

