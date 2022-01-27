using Abp.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHPQ.Services.DichVu
{
    public interface IUnlimitedSpaceServiceAppService : IApplicationService
    {
        Task<object> GetAll();
        Task<object> Create(UnlimitedSpaceServiceDto dto);
        Task<object> Update(UnlimitedSpaceServiceDto dto);
        Task<object> Delete(UnlimitedSpaceServiceDto dto);
    }
}
