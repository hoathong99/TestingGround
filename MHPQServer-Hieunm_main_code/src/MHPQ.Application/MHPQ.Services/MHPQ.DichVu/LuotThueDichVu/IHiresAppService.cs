using Abp.Application.Services;
using MHPQ.Common.DataResult;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHPQ.Services.DichVu
{
    public interface IHiresAppService : IApplicationService
    {
        Task<object> GetAll();
        Task<DataResult> GetEmptySpaceService(long limitedSpaceServiceId);
        Task<object> CreateHireLimitedSpace(HireServiceDto HireDto);
        Task<object> CreateHireUnlimitedSpace(HireServiceDto HireDto);
        void ChangeEmptySpace();
    }
}
