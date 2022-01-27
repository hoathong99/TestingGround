using Abp.Application.Services;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using MHPQ.Common.DataResult;
using MHPQ.Common.Enum;
using MHPQ.EntityDb;
using MHPQ.Services;
using MHPQ.Services.Dto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHPQ.MHPQ.Services.MHPQ.DichVu.Business
{

    public interface IBusinessGridViewAppRateService: IApplicationService
    {
        Task<object> GetRateDataAsync(GetRateInputDto input);
    }

    public class BusinessGridViewAppRateService: MHPQAppServiceBase, IBusinessGridViewAppRateService
    {
        private readonly IRepository<ObjectPartner, long> _objectPartnerRepos;
        private readonly IRepository<ObjectType, long> _objectTypeRepos;
        private readonly IRepository<Items, long> _itemsRepos;
        private readonly IRepository<ItemType, long> _itemTypeRepos;
        private readonly IRepository<Rate, long> _rateRepos;

        public BusinessGridViewAppRateService(
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
                             where rate.TenantId == null 
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
    }
}
