using Abp.AutoMapper;
using MHPQ.EntityDb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHPQ.Services.Dto
{


    [AutoMap(typeof(DeviceApi))]
    public class DeviceApiDto : DeviceApi
    {

    }
}
