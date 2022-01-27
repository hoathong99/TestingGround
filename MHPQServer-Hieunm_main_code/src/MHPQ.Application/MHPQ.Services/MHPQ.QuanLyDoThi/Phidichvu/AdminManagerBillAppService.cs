using Abp.Application.Services;
using Abp.AutoMapper;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using MHPQ.Common.DataResult;
using MHPQ.EntityDb;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHPQ.Services
{
    public interface IAdminManagerBillAppService : IApplicationService
    {
        Task<object> GetAllBillTypeAsync();
        Task<object> GetAllUserBillAsync();
        Task<object> GetBillTypeByIdAsync(long id);
        Task<object> GetAllBillViewSettingAsync(GetBillViewSettingInput input);
        Task<object> GetBillViewSettingsAsync(GetBillViewSettingInput input);
        Task<object> CreateOrUpdateUserBillAsync(UserBillDto input);
        Task<object> CreateOrUpdateBillTypeAsync(BillTypeDto input);
        Task<object> CreateOrUpdateBillViewSettingAsync(BillViewSettingDto input);
        Task<object> DeleteBillViewSettingAsync(long id);
        Task<object> DeleteUserBillAsync(long id);
        Task<object> DeleteBillTypeAsync(long id);

    }

    public class AdminManagerBillAppService : MHPQAppServiceBase, IAdminManagerBillAppService
    {
        private readonly IRepository<UserBill, long> _userBillRepos;
        private readonly IRepository<BillMappingType, long> _billTypeRepos;
        private readonly IRepository<BillViewSetting, long> _billViewSettingRepos;

        public AdminManagerBillAppService(
             IRepository<UserBill, long> userBillRepos,
             IRepository<BillMappingType, long> billTypeRepos,
             IRepository<BillViewSetting, long> billViewSettingRepos
            )
        {
            _userBillRepos = userBillRepos;
            _billTypeRepos = billTypeRepos;
            _billViewSettingRepos = billViewSettingRepos;
        }

        public async Task<object> GetAllBillTypeAsync()
        {
            try
            {
                long t1 = TimeUtils.GetNanoseconds();
                var result = await _billTypeRepos.GetAllListAsync();
                var data = DataResult.ResultSucces(result, "Get success");
                mb.statisticMetris(t1, 0, "admin_gall_itemtype");
                return data;
            }
            catch (Exception ex)
            {
                var data = DataResult.ResultError(ex.ToString(), "Có lỗi");
                Logger.Fatal(ex.Message, ex);
                return data;
            }
        }

        public async Task<object> GetAllBillViewSettingAsync(GetBillViewSettingInput input)
        {
            try
            {
                long t1 = TimeUtils.GetNanoseconds();
                var query = (from st in _billViewSettingRepos.GetAll()
                             where st.TenantId == AbpSession.TenantId
                             select new BillViewSettingDto()
                             {
                                 CreationTime = st.CreationTime,
                                 CreatorUserId = st.CreatorUserId,
                                 Id = st.Id,
                                 LastModificationTime = st.LastModificationTime,
                                 LastModifierUserId = st.LastModifierUserId,
                                 QueryKey = st.QueryKey,
                                 Properties = st.Properties,
                                 TenantId = st.TenantId,
                                 Type = st.Type
                             })
                             .WhereIf(input.Type.HasValue, x => x.Type == input.Type)
                             .WhereIf(input.Id.HasValue, x => x.Id == input.Id)
                             .AsQueryable();

                var result = await query.ToListAsync();

                var data = DataResult.ResultSucces(result, "Get success");
                mb.statisticMetris(t1, 0, "admin_gall_itemtype");
                return data;
            }
            catch (Exception ex)
            {
                var data = DataResult.ResultError(ex.ToString(), "Có lỗi");
                Logger.Fatal(ex.Message, ex);
                return data;
            }
        }

        public async Task<object> GetBillTypeByIdAsync(long id)
        {
            try
            {
                long t1 = TimeUtils.GetNanoseconds();
                var result = await _billTypeRepos.GetAllListAsync();
                var data = DataResult.ResultSucces(result, "Get success");
                mb.statisticMetris(t1, 0, "admin_gall_itemtype");
                return data;
            }
            catch (Exception ex)
            {
                var data = DataResult.ResultError(ex.ToString(), "Có lỗi");
                Logger.Fatal(ex.Message, ex);
                return data;
            }
        }

        public async Task<object> GetBillViewSettingsAsync(GetBillViewSettingInput input)
        {
            try
            {
                long t1 = TimeUtils.GetNanoseconds();
                var query = (from st in _billViewSettingRepos.GetAll()
                             where st.TenantId == AbpSession.TenantId
                             select new BillViewSettingDto()
                             {
                                 CreationTime = st.CreationTime,
                                 CreatorUserId =st.CreatorUserId,
                                 Id = st.Id,
                                 LastModificationTime = st.LastModificationTime,
                                 LastModifierUserId = st.LastModifierUserId,
                                 QueryKey = st.QueryKey,
                                 Properties = st.Properties,
                                 TenantId = st.TenantId,
                                 Type = st.Type
                             })
                             .WhereIf(input.Type.HasValue, x => x.Type == input.Type)
                             .WhereIf(input.Id.HasValue, x => x.Id == input.Id)
                             .AsQueryable();

                var result =  query.FirstOrDefault();
               
                var data = DataResult.ResultSucces(result, "Get success");
                mb.statisticMetris(t1, 0, "admin_gall_itemtype");
                return data;
            }
            catch (Exception ex)
            {
                var data = DataResult.ResultError(ex.ToString(), "Có lỗi");
                Logger.Fatal(ex.Message, ex);
                return data;
            }
        }

        [Obsolete]
        public async Task<object> CreateOrUpdateBillTypeAsync(BillTypeDto input)
        {
            try
            {
                long t1 = TimeUtils.GetNanoseconds();

                input.TenantId = AbpSession.TenantId;
                if (input.Id > 0)
                {
                    //update
                    var updateData = await _billTypeRepos.GetAsync(input.Id);
                    if (updateData != null)
                    {
                        input.MapTo(updateData);

                        //call back
                        await _billTypeRepos.UpdateAsync(updateData);
                    }
                    mb.statisticMetris(t1, 0, "admin_ud_billtype");

                    var data = DataResult.ResultSucces(updateData, "Update success !");
                    return data;
                }
                else
                {
                    //Insert
                    var insertInput = input.MapTo<BillMappingType>();
                    long id = await _billTypeRepos.InsertAndGetIdAsync(insertInput);

                    mb.statisticMetris(t1, 0, "admin_is_billtype");
                    var data = DataResult.ResultSucces(insertInput, "Insert success !");
                    return data;
                }


            }
            catch (Exception e)
            {
                var data = DataResult.ResultError(e.ToString(), "Exception");
                Logger.Fatal(e.Message);
                return data;
            }
        }

        [Obsolete]
        public async Task<object> CreateOrUpdateBillViewSettingAsync(BillViewSettingDto input)
        {
            try
            {
                long t1 = TimeUtils.GetNanoseconds();

                input.TenantId = AbpSession.TenantId;
                if (input.Id > 0)
                {
                    //update
                    var updateData = await  _billViewSettingRepos.GetAsync(input.Id);
                    if (updateData != null)
                    {
                        input.MapTo(updateData);

                        //call back
                        await _billViewSettingRepos.UpdateAsync(updateData);
                    }
                    mb.statisticMetris(t1, 0, "admin_ud_billvst");

                    var data = DataResult.ResultSucces(updateData, "Update success !");
                    return data;
                }
                else
                {
                    //Insert
                    var insertInput = input.MapTo<BillViewSetting>();
                    long id = await _billViewSettingRepos.InsertAndGetIdAsync(insertInput);

                    mb.statisticMetris(t1, 0, "admin_is_billvst");
                    var data = DataResult.ResultSucces(insertInput, "Insert success !");
                    return data;
                }


            }
            catch (Exception e)
            {
                var data = DataResult.ResultError(e.ToString(), "Exception");
                Logger.Fatal(e.Message);
                return data;
            }
        }

        [Obsolete]
        public async Task<object> CreateOrUpdateUserBillAsync(UserBillDto input)
        {
            try
            {
                long t1 = TimeUtils.GetNanoseconds();

                input.TenantId = AbpSession.TenantId;
                if (input.Id > 0)
                {
                    //update
                    var updateData = await _userBillRepos.GetAsync(input.Id);
                    if (updateData != null)
                    {
                        input.MapTo(updateData);

                        //call back
                        await _userBillRepos.UpdateAsync(updateData);
                    }
                    mb.statisticMetris(t1, 0, "admin_ud_usbill");

                    var data = DataResult.ResultSucces(updateData, "Update success !");
                    return data;
                }
                else
                {
                    //Insert
                    var insertInput = input.MapTo<UserBill>();
                    long id = await 
                        _userBillRepos.InsertAndGetIdAsync(insertInput);

                    mb.statisticMetris(t1, 0, "admin_is_usbill");
                    var data = DataResult.ResultSucces(insertInput, "Insert success !");
                    return data;
                }


            }
            catch (Exception e)
            {
                var data = DataResult.ResultError(e.ToString(), "Exception");
                Logger.Fatal(e.Message);
                return data;
            }
        }

        public async Task<object> DeleteBillTypeAsync(long id)
        {
            try
            {
                long t1 = TimeUtils.GetNanoseconds();
                await _billTypeRepos.DeleteAsync(id);
                var data = DataResult.ResultSucces("Delete Success");
                mb.statisticMetris(t1, 0, "admin_del_billtype");
                return data;
            }
            catch (Exception ex)
            {
                var data = DataResult.ResultError(ex.ToString(), "Có lỗi");
                Logger.Fatal(ex.Message, ex);
                return data;
            }
        }

        public async Task<object> DeleteBillViewSettingAsync(long id)
        {
            try
            {
                long t1 = TimeUtils.GetNanoseconds();
                await _billViewSettingRepos.DeleteAsync(id);
                var data = DataResult.ResultSucces("Delete Success");
                mb.statisticMetris(t1, 0, "admin_del_billvst");
                return data;
            }
            catch (Exception ex)
            {
                var data = DataResult.ResultError(ex.ToString(), "Có lỗi");
                Logger.Fatal(ex.Message, ex);
                return data;
            }
        }

        public async Task<object> DeleteUserBillAsync(long id)
        {
            try
            {
                long t1 = TimeUtils.GetNanoseconds();
                await _userBillRepos.DeleteAsync(id);
                var data = DataResult.ResultSucces("Delete Success");
                mb.statisticMetris(t1, 0, "admin_del_usbill");
                return data;
            }
            catch (Exception ex)
            {
                var data = DataResult.ResultError(ex.ToString(), "Có lỗi");
                Logger.Fatal(ex.Message, ex);
                return data;
            }
        }

        public async Task<object> GetAllUserBillAsync()
        {
            try
            {
                long t1 = TimeUtils.GetNanoseconds();
                var result = await _userBillRepos.GetAllListAsync();
                var data = DataResult.ResultSucces(result, "Get success");
                mb.statisticMetris(t1, 0, "admin_gall_itemtype");
                return data;
            }
            catch (Exception ex)
            {
                var data = DataResult.ResultError(ex.ToString(), "Có lỗi");
                Logger.Fatal(ex.Message, ex);
                return data;
            }
        }
    }
}
