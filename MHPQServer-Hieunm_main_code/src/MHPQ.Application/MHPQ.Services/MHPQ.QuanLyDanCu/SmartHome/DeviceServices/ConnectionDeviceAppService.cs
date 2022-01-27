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
    public class ConnectionDeviceAppService : MHPQDeviceServiceBase<ConnectionDevice, long>
    {
        public ConnectionDeviceAppService(IRepository<ConnectionDevice, long> connectionDeviceRepo) : base(connectionDeviceRepo)
        {

        }
    }
}
