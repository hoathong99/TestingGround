using Abp.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHPQ.Services.DichVu
{
    public interface ILimitedSpaceServiceAppService : IApplicationService
    {
        Task<object> GetAll();
        Task<object> Create(LimitedSpaceServiceDto dto);
        Task<object> Update(LimitedSpaceServiceDto dto);
        Task<object> Delete(LimitedSpaceServiceDto dto);
    }
}
