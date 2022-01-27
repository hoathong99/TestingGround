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
    public interface IAirDeviceAppService
    {

    }
    public class AirDeviceAppService : MHPQDeviceServiceBase<AirDevice, long>, IAirDeviceAppService
    {
        public AirDeviceAppService(IRepository<AirDevice, long> airDeviceRepo) : base(airDeviceRepo)
        {
        }
    }
}
