using Abp.Application.Services;
using Abp.Domain.Repositories;
using MHPQ.EntityDb;
using MHPQ.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHPQ.Services
{
    public interface ILightDeviceAppService : IApplicationService
    {
    }

    public class LightDeviceAppService : MHPQDeviceServiceBase<LightDevice, long>, ILightDeviceAppService
    {

        public LightDeviceAppService(IRepository<LightDevice, long> lightDeviceRepo) : base(lightDeviceRepo)
        {
        }
        

    }
}
