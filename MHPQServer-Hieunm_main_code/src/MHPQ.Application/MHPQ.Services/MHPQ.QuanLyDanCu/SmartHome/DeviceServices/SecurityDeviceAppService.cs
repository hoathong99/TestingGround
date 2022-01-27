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
    public class SecurityDeviceAppService : MHPQDeviceServiceBase<SecurityDevice, long>
    {

        public SecurityDeviceAppService(IRepository<SecurityDevice, long> securityDeviceRepo) : base(securityDeviceRepo)
        {
        }


    }
}
