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
    public class WaterDeviceAppService : MHPQDeviceServiceBase<WatterDevice, long>
    {

        public WaterDeviceAppService(IRepository<WatterDevice, long> waterDeviceRepo) : base(waterDeviceRepo)
        {
        }


    }
}
