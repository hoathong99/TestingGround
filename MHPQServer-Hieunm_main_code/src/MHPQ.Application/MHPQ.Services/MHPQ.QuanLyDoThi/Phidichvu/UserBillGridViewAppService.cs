

using Abp.Application.Services;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using MHPQ.Common.DataResult;
using MHPQ.Common.Enum;
using MHPQ.EntityDb;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MHPQ.Services
{
    public interface IUserBillGridViewAppService : IApplicationService
    {
        Task<object> GetUserBillAsync(BillGridViewInput input);
    }

    public class UserBillGridViewAppService : MHPQAppServiceBase, IUserBillGridViewAppService
    {
        private readonly IRepository<UserBill, long> _userBillRepos;

        public UserBillGridViewAppService(
             IRepository<UserBill, long> userBillRepos
            )
        {
            _userBillRepos = userBillRepos;
        }
        protected IQueryable<UserBillDto> QueryGetAllData(BillGridViewInput input)
        {
            try
            {
                //  DateTime fromDay = new DateTime(), toDay = new DateTime();
                DateTime currentDay = DateTime.Now;
                DateTime currentMonth = new DateTime(currentDay.Year, currentDay.Month, 1);

                var query = (from ul in _userBillRepos.GetAll()
                             select new UserBillDto()
                             {
                                 Id = ul.Id,
                                 CreationTime = ul.CreationTime,
                                 CreatorUserId = ul.CreatorUserId,
                                 LastModificationTime = ul.LastModificationTime,
                                 LastModifierUserId = ul.LastModifierUserId,
                                 Name = ul.Name,
                                 Properties = ul.Properties,
                                 TenantId = ul.TenantId,
                                 Type = ul.Type
                                 
                             })
                             .WhereIf(!string.IsNullOrEmpty(input.Keyword), u => u.QueryKey.Contains(input.Keyword) || u.Name.Contains(input.Keyword) || u.Properties.Contains(input.Keyword))
                             .WhereIf(input.Id.HasValue, u => u.Id == input.Id)
                             .WhereIf(input.Type.HasValue, u => u.Type == input.Type)
                             .AsQueryable();

                #region Truy van tung Form

                switch (input.FormId)
                {
                    //cua hang moi dang ky
                    case (int)CommonENumBill.FORM_ID_BILL.FORM_ADMIN_GETALL:

                        break;
                    case (int)CommonENumBill.FORM_ID_BILL.FORM_ADMIN_GETALL_BY_MONTH:
                        query = query.Where(x => (x.CreationTime.Month == input.Month.Value && x.CreationTime.Year == input.Year.Value));
                        break;
                    case (int)CommonENumBill.FORM_ID_BILL.FORM_ADMIN_GETALL_NEW_MONTH:
                        query = query.Where(x => (x.CreationTime.Month == currentDay.Month && x.CreationTime.Year == currentDay.Year));
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

        public async Task<object> GetUserBillAsync(BillGridViewInput input)
        {
            try
            {
                long t1 = TimeUtils.GetNanoseconds();
                var query = QueryGetAllData(input);
                int numberData = 0;
                var obj = new object();
                if (input.FormCase == null || input.FormCase == 1)
                {
                    numberData = query.Count();
                    obj = await query.Skip(input.SkipCount).Take(input.MaxResultCount).ToListAsync();
                }
                else if (input.FormCase == 2)
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
