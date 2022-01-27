using Abp.AutoMapper;
using MHPQ.EntityDb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHPQ.Services.Dto
{
    public class HomeServerInput
    {
        public long Id { get; set; }
    }

    [AutoMap(typeof(HomeServer))]
    public class HomeServerDto : HomeServer
    {

    }

    
}
