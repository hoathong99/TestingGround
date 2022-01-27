using System.Threading.Tasks;
using Abp.Application.Services;
using MHPQ.Sessions.Dto;

namespace MHPQ.Sessions
{
    public interface ISessionAppService : IApplicationService
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations();
    }
}
