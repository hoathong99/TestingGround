using Abp.AutoMapper;
using MHPQ.EntityDb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHPQ.Service.Dto
{
    [AutoMap(typeof(HomeGateway))]
    public class HomeGatewayDto : HomeGateway
    {
    }

   


}
