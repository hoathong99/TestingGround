using Abp.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHPQ.Services.DichVu
{
    public interface IGuestServiceAppService : IApplicationService
    {
        Task<object> GetAll();
        Task<object> Create(GuestServiceDto guestDto);
        Task<object> Update(GuestServiceDto guestDto);
        Task<object> Delete(GuestServiceDto guestDto);

    }
}
