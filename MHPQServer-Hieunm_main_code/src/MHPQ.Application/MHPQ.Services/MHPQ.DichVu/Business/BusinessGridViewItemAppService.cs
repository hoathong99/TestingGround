using Abp.Application.Services;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Abp.Runtime.Session;
using MHPQ.Common.DataResult;
using MHPQ.Common.Enum;
using MHPQ.EntityDb;
using MHPQ.Services.Dto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHPQ.Services
{
    public interface IBusinessGridViewItemtAppService : IApplicationService
    {
        Task<object> GetItemDataAsync(GetItemInputDto input);

    }
    public class BusinessGridViewItemtAppService : MHPQAppServiceBase, IBusinessGridViewItemtAppService
    {

        private readonly IRepository<ObjectPartner, long> _objectPartnerRepos;
        private readonly IRepository<ObjectType, long> _objectTypeRepos;
        private readonly IRepository<Items, long> _itemsRepos;
        private readonly IRepository<ItemType, long> _itemTypeRepos;
        private readonly IRepository<Rate, long> _rateRepos;

        private static ConcurrentDictionary<string, IQueryable<ItemsDto>> listQuery = new ConcurrentDictionary<string, IQueryable<ItemsDto>>();

        public BusinessGridViewItemtAppService(
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
                             where item.TenantId == null && item.Type == input.Type
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
                                                                 select rate.RatePoint),1),
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
                        query = query.Where(x => ( x.State == (int)CommonENumItem.STATE_ITEM.ACTIVE) && x.ObjectPartnerId == input.ObjectPartnerId);
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
                int numberData = 0 ;
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


    }
}
