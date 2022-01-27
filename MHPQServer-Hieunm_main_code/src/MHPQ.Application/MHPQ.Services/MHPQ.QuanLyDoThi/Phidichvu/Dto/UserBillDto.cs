using Abp.AutoMapper;
using MHPQ.EntityDb;


namespace MHPQ.Services
{
    [AutoMap(typeof(UserBill))]
    public class UserBillDto : UserBill
    {
    }

    [AutoMap(typeof(BillMappingType))]
    public class BillTypeDto : BillMappingType
    {
    }

    [AutoMap(typeof(BillViewSetting))]
    public class BillViewSettingDto : BillViewSetting
    {
    }
}
