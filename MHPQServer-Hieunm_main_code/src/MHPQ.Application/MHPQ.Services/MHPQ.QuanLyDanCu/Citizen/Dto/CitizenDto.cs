

using Abp.AutoMapper;
using MHPQ.EntityDb;

namespace MHPQ.Services
{
    [AutoMap(typeof(Citizen))]
    public class CitizenDto : Citizen
    {
    }

    public class AccountDto
    {
        public long Id { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string ImageUrl { get; set; }
    }

    public class SmarthomeTenantDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string SmarthomeCode { get; set; }
        public string ImageUrl { get; set; }
    }

}
