using Abp.Domain.Repositories;
using MHPQ.EntityDb;


namespace MHPQ.Services
{
        public class FireAlarmDeviceAppService : MHPQDeviceServiceBase<FireAlarmDevice, long>
        {
            public FireAlarmDeviceAppService(IRepository<FireAlarmDevice, long> fireAlarmDeviceRepo) : base(fireAlarmDeviceRepo)
            {

            }
        }
}
