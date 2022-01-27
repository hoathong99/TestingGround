using Abp.Application.Services;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using GeoCoordinatePortable;
using MHPQ.Common.DataResult;
using MHPQ.Common.Enum;
using MHPQ.EntityDb;
using MHPQ.Services.Dto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHPQ.MHPQ.Services.MHPQ.DichVu.Business
{
    public interface IBusinessGridViewObjectAppService : IApplicationService
    {
        Task<object> GetObjectDataAsync(GetObjectInputDto input);
       
    }

    public class BusinessGridViewObjectAppService: MHPQAppServiceBase, IBusinessGridViewObjectAppService
    {

        private readonly IRepository<ObjectPartner, long> _objectPartnerRepos;
        private readonly IRepository<ObjectType, long> _objectTypeRepos;
        private readonly IRepository<Items, long> _itemsRepos;
        private readonly IRepository<ItemType, long> _itemTypeRepos;
        private readonly IRepository<Rate, long> _rateRepos;

        public BusinessGridViewObjectAppService(
            IRepository<ObjectPartner, long> objectPartnerRepos,
            IRepository<ObjectType, long> objectTypeRepos,
            IRepository<Items, long> itemsRepos,
            IRepository<ItemType, long> itemTypeRepos,
            IRepository<Rate, long> rateRepos
            )
        {
            _objectPartnerRepos = objectPartnerRepos;
            _objectTypeRepos = objectTypeRepos;
            _itemsRepos = itemsRepos;
            _itemTypeRepos = itemTypeRepos;
            _rateRepos = rateRepos;
        }

        protected double CalculateDistance(Location point1, Location point2)
        {
            var d1 = point1.Latitude * (Math.PI / 180.0);
            var num1 = point1.Longitude * (Math.PI / 180.0);
            var d2 = point2.Latitude * (Math.PI / 180.0);
            var num2 = point2.Longitude * (Math.PI / 180.0) - num1;
            var d3 = Math.Pow(Math.Sin((d2 - d1) / 2.0), 2.0) +
                     Math.Cos(d1) * Math.Cos(d2) * Math.Pow(Math.Sin(num2 / 2.0), 2.0);
            return 6376500.0 * (2.0 * Math.Atan2(Math.Sqrt(d3), Math.Sqrt(1.0 - d3)));
        }

        protected IQueryable<ObjectDto> QueryGetAllData(GetObjectInputDto input)
        {
            try
            {

                var coord = ( input.Latitude.HasValue && input.Longitude.HasValue ) ? new GeoCoordinate(input.Latitude.Value, input.Longitude.Value) : null;
                DateTime fromDay = new DateTime(), toDay = new DateTime();
                if (input.FromDay.HasValue)
                {
                    fromDay = new DateTime(input.FromDay.Value.Year, input.FromDay.Value.Month, input.FromDay.Value.Day, 0, 0, 0);
                  
                }
                if (input.ToDay.HasValue)
                {
                    toDay = new DateTime(input.ToDay.Value.Year, input.ToDay.Value.Month, input.ToDay.Value.Day, 23, 59, 59);

                }
                var startPoint = new { Latitude = 1.123, Longitude = 12.3 };

              
                var query = (from obj in _objectPartnerRepos.GetAll()
                             where obj.TenantId == null && obj.Type == input.Type
                             let dt = (input.Latitude.HasValue && input.Longitude.HasValue && obj.Longitude.HasValue && obj.Latitude.HasValue) ? (new GeoCoordinate { Latitude = obj.Latitude.Value, Longitude = obj.Longitude.Value }.GetDistanceTo(coord)) / 1000.0 : 10000
                             select new ObjectDto()
                             {
                                 Id = obj.Id,
                                 CreationTime = obj.CreationTime,
                                 CreatorUserId = obj.CreatorUserId,
                                 DeleterUserId = obj.DeleterUserId,
                                 DeletionTime = obj.DeletionTime,
                                 IsDeleted = obj.IsDeleted,
                                 LastModificationTime = obj.LastModificationTime,
                                 LastModifierUserId = obj.LastModifierUserId,
                                 Like = obj.Like,
                                 Name = obj.Name,
                                 Operator = obj.Operator,
                                 Owner = obj.Owner,
                                 Properties = obj.Properties,
                                 QueryKey = obj.QueryKey,
                                 PropertyHistories = obj.PropertyHistories,
                                 StateProperties = obj.StateProperties,
                                 Type = obj.Type,
                                 State = obj.State,
                                 Latitude = obj.Latitude,
                                 Longitude = obj.Longitude,
                                 
                                 Distance = dt,
                                 Rates = (from rate in _rateRepos.GetAll()
                                          where rate.ObjectId == obj.Id && rate.ItemId == null
                                          select rate)
                                          .ToList(),
                                 CountRate = (from rate in _rateRepos.GetAll()
                                              where rate.ObjectId == obj.Id && rate.ItemId == null
                                              select rate).AsQueryable().Count(),
                                 Rate = (float)Math.Round((float)Queryable.Average(from rate in _rateRepos.GetAll()
                                                                                   where rate.ObjectId == obj.Id && rate.ItemId == null                                                                                   
                                                                                   select rate.RatePoint), 1),
                                 Items = (input.FormCase == (int)CommonENumObject.FORM_ID_OBJECT.FORM_PARTNER_OBJECT_DETAIL) ? (
                                  (from it in _itemsRepos.GetAll()
                                   where it.ObjectPartnerId == obj.Id
                                   select it).Take(10).ToList()
                                 ) : null

                             })
                             .WhereIf(!string.IsNullOrEmpty(input.Keyword), u => u.QueryKey.Contains(input.Keyword) || u.Name.Contains(input.Keyword) || u.Properties.Contains(input.Keyword))
                             .WhereIf(input.Id.HasValue, u => u.Id == input.Id)
                             .WhereIf(input.FromDay.HasValue, u =>(u.LastModificationTime.HasValue && u.LastModificationTime >= fromDay) || (!u.LastModificationTime.HasValue && u.CreationTime >= fromDay))
                             .AsQueryable();

                #region Data Common
                #endregion
                #region Truy van tung Form
               
                switch(input.FormId)
                {
                    //cua hang moi dang ky
                    case (int)CommonENumObject.FORM_ID_OBJECT.FORM_ADMIN_GET_OBJECT_NEW:
                         query = query.Where(x => x.State == null || x.State == (int)CommonENumObject.STATE_OBJECT.NEW).OrderBy(x => x.CreationTime);
                         break;
                    //cua hang da duoc duyet
                    case (int)CommonENumObject.FORM_ID_OBJECT.FORM_ADMIN_GET_OBJECT_ACTIVE:
                        query = query.Where(x => x.State != null && x.State == (int)CommonENumObject.STATE_OBJECT.ACTIVE).OrderBy(x => x.CreationTime);
                        break;
                    case (int)CommonENumObject.FORM_ID_OBJECT.FORM_ADMIN_GET_OBJECT_GETALL:
                        break;
                    case (int)CommonENumObject.FORM_ID_OBJECT.FORM_ADMIN_OBJECT_REFUSE:
                        query = query.Where(x => x.State != null && x.State == (int)CommonENumObject.STATE_OBJECT.REFUSE).OrderBy(x => x.CreationTime);
                        break;
                    case (int)CommonENumObject.FORM_ID_OBJECT.FORM_ADMIN_OBJECT_DISABLE:
                        query = query.Where(x => x.State != null && x.State == (int)CommonENumObject.STATE_OBJECT.DISABLE).OrderBy(x => x.CreationTime);
                        break;
                    case (int)CommonENumObject.FORM_ID_OBJECT.FORM_PARTNER_OBJECT_GETALL:
                        query = query.Where(x => x.CreatorUserId == AbpSession.UserId);
                        break;
                    case (int)CommonENumObject.FORM_ID_OBJECT.FORM_PARTNER_OBJECT_NEW:
                        query = query.Where(x => (x.State == null || x.State == (int)CommonENumObject.STATE_OBJECT.NEW) && x.CreatorUserId == AbpSession.UserId);
                        break;
                    case (int)CommonENumObject.FORM_ID_OBJECT.FORM_PARTNER_OBJECT_DETAIL:
                        query = query.Where(x => x.CreatorUserId == AbpSession.UserId);
                        break;
                    case (int)CommonENumObject.FORM_ID_OBJECT.FORM_USER_OBJECT_GETALL:
                        query = query.Where(x =>x.State == (int)CommonENumObject.STATE_OBJECT.ACTIVE).OrderBy(x => x.CreationTime);
                        break;
                    case (int)CommonENumObject.FORM_ID_OBJECT.FORM_USER_OBJECT_HIGHT_RATE:
                        query = query.Where(x => x.State == (int)CommonENumObject.STATE_OBJECT.ACTIVE).OrderByDescending(x => x.Rate);
                        break;
                    case (int)CommonENumObject.FORM_ID_OBJECT.FORM_USER_OBJECT_LOW_RATE:
                        query = query.Where(x => x.State == (int)CommonENumObject.STATE_OBJECT.ACTIVE).OrderByDescending(x => x.Rate.HasValue).ThenBy(x => x.Rate);
                        break;
                    case (int)CommonENumObject.FORM_ID_OBJECT.FORM_USER_OBJECT_DETAIL:
                        //query = (from obj in query
                        //         join pd in _itemsRepos.GetAll() on obj.Id equals pd.ObjectPartnerId
                        //         select obj 
                        //         )
                        break;
                    case (int)CommonENumObject.FORM_ID_OBJECT.FORM_USER_OBJECT_LOCATIONMAP:
                        query = query.Where(x => x.State == (int)CommonENumObject.STATE_OBJECT.ACTIVE);
                        break;

                }

                #endregion

                return query;
            }
            catch (Exception ex)
            {
                Logger.Fatal(ex.Message);
                return null;
            }
        }

        public async Task<object> GetObjectDataAsync(GetObjectInputDto input)
        {
            try
            {
               
                long t1 = TimeUtils.GetNanoseconds();
                var obj = new  object();
                var query = QueryGetAllData(input);
                var count = query.Count();
                if (input.FormCase == null || input.FormCase == (int)CommonENumObject.FORMCASE_GET_DATA.OBJECT_GETALL)
                {
                    if(input.FormId == (int)CommonENumObject.FORM_ID_OBJECT.FORM_USER_OBJECT_LOCATIONMAP)
                    {
                        var list = await query.ToListAsync();

                        obj = list.OrderBy(x => x.Distance).Skip(input.SkipCount).Take(input.MaxResultCount);
                    }
                    else
                    {
                        var list = await query.Skip(input.SkipCount).Take(input.MaxResultCount).ToListAsync();

                        obj = list;
                    }
                    
                  
                }
                else if(input.FormCase == (int)CommonENumObject.FORMCASE_GET_DATA.OBJECT_DETAIL)
                {
                    obj = await query.FirstOrDefaultAsync();
                }

                var data = DataResult.ResultSucces(obj, "Get success");
                mb.statisticMetris(t1, 0, "gall_obj");
                return data;
            }
            catch(Exception ex)
            {
                var data = DataResult.ResultError(ex.ToString(), "Exception");
                Logger.Fatal(ex.Message, ex);
                return data;
            }
        }


    }
}
