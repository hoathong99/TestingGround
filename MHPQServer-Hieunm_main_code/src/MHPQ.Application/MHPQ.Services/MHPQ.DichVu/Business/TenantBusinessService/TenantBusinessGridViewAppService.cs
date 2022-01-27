using Abp.Application.Services;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using MHPQ.Common.DataResult;
using MHPQ.Common.Enum;
using MHPQ.EntityDb;
using MHPQ.Services.Dto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHPQ.Services
{
    public interface ITenantBusinessGridViewAppService : IApplicationService
    {
        #region Object
        Task<object> GetObjectDataAsync(GetObjectInputDto input);
        #endregion
        #region Item
        Task<object> GetItemDataAsync(GetItemInputDto input);
        #endregion
        #region Rate
        Task<object> GetRateDataAsync(GetRateInputDto input);
        #endregion
    }

    public class TenantBusinessGridViewAppService : MHPQAppServiceBase, ITenantBusinessGridViewAppService
    {

        private readonly IRepository<ObjectPartner, long> _objectPartnerRepos;
        private readonly IRepository<ObjectType, long> _objectTypeRepos;
        private readonly IRepository<Items, long> _itemsRepos;
        private readonly IRepository<ItemType, long> _itemTypeRepos;
        private readonly IRepository<Rate, long> _rateRepos;

        public TenantBusinessGridViewAppService(
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

                var coord = (input.Latitude.HasValue && input.Longitude.HasValue) ? new GeoCoordinate(input.Latitude.Value, input.Longitude.Value) : null;
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
                             where obj.Type == input.Type
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
                             .WhereIf(input.FromDay.HasValue, u => (u.LastModificationTime.HasValue && u.LastModificationTime >= fromDay) || (!u.LastModificationTime.HasValue && u.CreationTime >= fromDay))
                             .AsQueryable();

                #region Data Common
                #endregion
                #region Truy van tung Form

                switch (input.FormId)
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
                        query = query.Where(x => x.State == (int)CommonENumObject.STATE_OBJECT.ACTIVE).OrderBy(x => x.CreationTime);
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
                var obj = new object();
                var query = QueryGetAllData(input);
                var count = query.Count();
                if (input.FormCase == null || input.FormCase == (int)CommonENumObject.FORMCASE_GET_DATA.OBJECT_GETALL)
                {
                    if (input.FormId == (int)CommonENumObject.FORM_ID_OBJECT.FORM_USER_OBJECT_LOCATIONMAP)
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
                else if (input.FormCase == (int)CommonENumObject.FORMCASE_GET_DATA.OBJECT_DETAIL)
                {
                    obj = await query.FirstOrDefaultAsync();
                }

                var data = DataResult.ResultSucces(obj, "Get success");
                mb.statisticMetris(t1, 0, "gall_obj");
                return data;
            }
            catch (Exception ex)
            {
                var data = DataResult.ResultError(ex.ToString(), "Exception");
                Logger.Fatal(ex.Message, ex);
                return data;
            }
        }

        #region Item
        protected IQueryable<ItemsDto> QueryGetAllData(GetItemInputDto input)
        {
            try
            {
                DateTime fromDay = new DateTime(), toDay = new DateTime();
                if (input.FromDay.HasValue)
                {
                    fromDay = new DateTime(input.FromDay.Value.Year, input.FromDay.Value.Month, input.FromDay.Value.Day, 0, 0, 0);

                }
                if (input.ToDay.HasValue)
                {
                    toDay = new DateTime(input.ToDay.Value.Year, input.ToDay.Value.Month, input.ToDay.Value.Day, 23, 59, 59);

                }

                var query = (from item in _itemsRepos.GetAll()
                             join obj in _objectPartnerRepos.GetAll() on item.ObjectPartnerId equals obj.Id into tb_obj
                             from obj in tb_obj.DefaultIfEmpty()
                             where item.Type == input.Type
                             select new ItemsDto()
                             {
                                 Id = item.Id,
                                 CreationTime = item.CreationTime,
                                 CreatorUserId = item.CreatorUserId,
                                 DeleterUserId = item.DeleterUserId,
                                 DeletionTime = item.DeletionTime,
                                 IsDeleted = item.IsDeleted,
                                 LastModificationTime = item.LastModificationTime,
                                 LastModifierUserId = item.LastModifierUserId,
                                 Like = item.Like,
                                 Name = item.Name,
                                 ShopName = obj.Name,
                                 Properties = item.Properties,
                                 QueryKey = item.QueryKey,
                                 PropertyHistories = item.PropertyHistories,
                                 StateProperties = item.StateProperties,
                                 Type = item.Type,
                                 TypeGoods = item.TypeGoods,
                                 ObjectPartnerId = item.ObjectPartnerId,
                                 Category = item.Category,
                                 State = item.State,
                                 Rates = (from rate in _rateRepos.GetAll()
                                          where rate.ItemId == item.Id
                                          select rate).ToList(),
                                 CountRate = (from rate in _rateRepos.GetAll()
                                              where rate.ItemId == item.Id
                                              select rate).AsQueryable().Count(),
                                 Rate = (float)System.Math.Round((float)Queryable.Average(from rate in _rateRepos.GetAll()
                                                                                          where rate.ItemId == item.Id
                                                                                          select rate.RatePoint), 1),
                                 //Items = (input.FormCase == (int)CommonENumObject.FORMCASE_GET_DATA.OBJECT_DETAIL) ? (
                                 // (from it in _itemsRepos.GetAll()
                                 //  where it.ObjectPartnerId == obj.Id
                                 //  select it).ToList()
                                 //) : null

                             })
                             .WhereIf(!string.IsNullOrEmpty(input.Keyword), u => u.QueryKey.Contains(input.Keyword) || u.Name.Contains(input.Keyword) || u.Properties.Contains(input.Keyword))
                             .WhereIf(input.TypeGoods > 0, u => u.TypeGoods == input.TypeGoods)
                             .WhereIf(input.Id > 0, u => u.Id == input.Id)
                             .WhereIf(input.FromDay.HasValue, u => (u.LastModificationTime.HasValue && u.LastModificationTime >= fromDay) || (!u.LastModificationTime.HasValue && u.CreationTime >= fromDay))
                             .AsQueryable();

                #region Data Common
                #endregion
                #region Truy van tung Form

                switch (input.FormId)
                {
                    //san pham moi dang ky
                    case (int)CommonENumItem.FORM_ID_ITEM.FORM_ADMIN_GET_ITEM_NEW:
                        query = query.Where(x => x.State == null || x.State == (int)CommonENumItem.STATE_ITEM.NEW);
                        break;
                    //san pham da duoc duyet
                    case (int)CommonENumItem.FORM_ID_ITEM.FORM_ADMIN_GET_ITEM_ACTIVE:
                        query = query.Where(x => x.State != null && x.State == (int)CommonENumItem.STATE_ITEM.ACTIVE);
                        break;
                    case (int)CommonENumItem.FORM_ID_ITEM.FORM_ADMIN_GET_ITEM_GETALL:
                        break;
                    case (int)CommonENumItem.FORM_ID_ITEM.FORM_ADMIN_ITEM_REFUSE:
                        query = query.Where(x => x.State != null && x.State == (int)CommonENumItem.STATE_ITEM.REFUSE);
                        break;
                    case (int)CommonENumItem.FORM_ID_ITEM.FORM_ADMIN_ITEM_DISABLE:
                        query = query.Where(x => x.State != null && x.State == (int)CommonENumItem.STATE_ITEM.DISABLE);
                        break;
                    case (int)CommonENumItem.FORM_ID_ITEM.FORM_PARTNER_ITEM_GETALL:
                        query = query.Where(x => x.ObjectPartnerId == input.ObjectPartnerId);
                        break;
                    case (int)CommonENumItem.FORM_ID_ITEM.FORM_PARTNER_ITEM_ACTIVE:
                        query = query.Where(x => (x.State == (int)CommonENumItem.STATE_ITEM.ACTIVE) && x.ObjectPartnerId == input.ObjectPartnerId);
                        break;
                    case (int)CommonENumItem.FORM_ID_ITEM.FORM_PARTNER_ITEM_NEW:
                        query = query.Where(x => (x.State == null || x.State == (int)CommonENumItem.STATE_ITEM.NEW) && x.ObjectPartnerId == input.ObjectPartnerId);
                        break;
                    case (int)CommonENumItem.FORM_ID_ITEM.FORM_PARTNER_ITEM_DETAIL:
                        query = query.Where(x => x.ObjectPartnerId == input.ObjectPartnerId);
                        break;
                    case (int)CommonENumItem.FORM_ID_ITEM.FORM_PARTNER_ITEM_DISABLE:
                        query = query.Where(x => (x.State == (int)CommonENumItem.STATE_ITEM.DISABLE) && x.ObjectPartnerId == input.ObjectPartnerId);
                        break;
                    case (int)CommonENumItem.FORM_ID_ITEM.FORM_PARTNER_ITEM_REFUSE:
                        query = query.Where(x => (x.State == (int)CommonENumItem.STATE_ITEM.DISABLE) && x.ObjectPartnerId == input.ObjectPartnerId);
                        break;
                    case (int)CommonENumItem.FORM_ID_ITEM.FORM_USER_ITEM_GETALL:
                        query = query.Where(x => x.State == (int)CommonENumItem.STATE_ITEM.ACTIVE).OrderByDescending(x => x.CreationTime);
                        break;
                    case (int)CommonENumItem.FORM_ID_ITEM.FORM_USER_ITEM_HIGHT_RATE:
                        query = query.Where(x => x.State == (int)CommonENumItem.STATE_ITEM.ACTIVE).OrderByDescending(x => x.Rate);
                        break;
                    case (int)CommonENumItem.FORM_ID_ITEM.FORM_USER_ITEM_LOW_RATE:
                        query = query.Where(x => x.State == (int)CommonENumItem.STATE_ITEM.ACTIVE).OrderByDescending(x => x.Rate.HasValue).ThenBy(x => x.Rate);
                        break;
                    case (int)CommonENumItem.FORM_ID_ITEM.FORM_USER_ITEM_DETAIL:
                        //query = (from obj in query
                        //         join pd in _itemsRepos.GetAll() on obj.Id equals pd.ObjectPartnerId
                        //         select obj 
                        //         )
                        break;
                    case (int)CommonENumItem.FORM_ID_ITEM.FORM_USER_ITEM_GETALL_BY_OBJECT:
                        query = query.Where(x => x.ObjectPartnerId == input.ObjectPartnerId);
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

        public async Task<object> GetItemDataAsync(GetItemInputDto input)
        {
            try
            {
                long t1 = TimeUtils.GetNanoseconds();
                var query = QueryGetAllData(input);
                int numberData = 0;
                var obj = new object();
                if (input.FormCase == null || input.FormCase == (int)CommonENumItem.FORMCASE_GET_DATA.GETALL_ITEM)
                {
                    numberData = query.Count();
                    obj = await query.Skip(input.SkipCount).Take(input.MaxResultCount).ToListAsync();
                }
                else if (input.FormCase == (int)CommonENumItem.FORMCASE_GET_DATA.GET_DETAIL_ITEM)
                {
                    obj = await query.FirstOrDefaultAsync();
                }

                var data = DataResult.ResultSucces(obj, "Get success", numberData);
                mb.statisticMetris(t1, 0, "gall_obj");
                return data;
            }
            catch (Exception ex)
            {
                var data = DataResult.ResultError(ex.ToString(), "Exception");
                Logger.Fatal(ex.Message, ex);
                return data;
            }
        }

        #endregion

        #region Rate
        protected IQueryable<RateDto> QueryGetAllData(GetRateInputDto input)
        {
            try
            {
                DateTime fromDay = new DateTime(), toDay = new DateTime();
                if (input.FromDay.HasValue)
                {
                    fromDay = new DateTime(input.FromDay.Value.Year, input.FromDay.Value.Month, input.FromDay.Value.Day, 0, 0, 0);

                }
                if (input.ToDay.HasValue)
                {
                    toDay = new DateTime(input.ToDay.Value.Year, input.ToDay.Value.Month, input.ToDay.Value.Day, 23, 59, 59);

                }
                var query = (from rate in _rateRepos.GetAll()
                                 //join obj in _objectPartnerRepos.GetAll() on rate.ObjectId equals obj.Id into tb_obj
                                 //from obj in tb_obj.DefaultIfEmpty()
                             join item in _itemsRepos.GetAll() on rate.ItemId equals item.Id into tb_item
                             from item in tb_item.DefaultIfEmpty()
                             join aswrt in _rateRepos.GetAll() on rate.Id equals aswrt.AnswerRateId into tb_aswrt
                             from aswrt in tb_aswrt.DefaultIfEmpty()
                             select new RateDto()
                             {
                                 ItemId = rate.ItemId,
                                 Comment = rate.Comment,
                                 CreationTime = rate.CreationTime,
                                 CreatorUserId = rate.CreatorUserId,
                                 DeleterUserId = rate.DeleterUserId,
                                 DeletionTime = rate.DeletionTime,
                                 Email = rate.Email,
                                 Id = rate.Id,
                                 Answerd = aswrt,
                                 HasAnswered = aswrt != null ? true : false,
                                 LastModificationTime = rate.LastModificationTime,
                                 LastModifierUserId = rate.LastModifierUserId,
                                 ObjectId = rate.ObjectId,
                                 RatePoint = rate.RatePoint,
                                 UserName = rate.UserName,
                                 IsItemReview = item != null ? true : false,
                                 Item = new ItemsDto()
                                 {
                                     Name = item.Name,
                                     Properties = item.Properties
                                 }
                             })
                             .WhereIf(input.ItemId != null, x => x.ItemId == input.ItemId)
                             .AsQueryable();

                #region Data Common
                #endregion
                #region Truy van tung Form
                switch (input.FormId)
                {
                    //san pham moi dang ky
                    case (int)CommonENumRate.FORM_ID_RATE.FORM_PARTNER_GETALL_REVIEW:
                        query = query.Where(x => x.ObjectId == input.ObjectPartnerId);
                        break;
                    case (int)CommonENumRate.FORM_ID_RATE.FORM_USER_GETALL_REVIEW_BY_PRODUCT:
                        query = query.Where(x => x.ItemId == input.ItemId);
                        break;
                    case (int)CommonENumRate.FORM_ID_RATE.FORM_USER_GETALL_REVIEW_BY_SHOP:
                        query = query.Where(x => x.ObjectId == input.ObjectPartnerId && x.ItemId == null);
                        break;
                    default:
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

        public async Task<object> GetRateDataAsync(GetRateInputDto input)
        {
            try
            {

                long t1 = TimeUtils.GetNanoseconds();
                var obj = new object();
                var query = QueryGetAllData(input);
                var count = query.Count();
                var list = await query.ToListAsync();

                var data = DataResult.ResultSucces(list, "Get success");
                mb.statisticMetris(t1, 0, "gall_obj");
                return data;
            }
            catch (Exception ex)
            {
                var data = DataResult.ResultError(ex.ToString(), "Exception");
                Logger.Fatal(ex.Message, ex);
                return data;
            }
        }
        #endregion

    }
}
