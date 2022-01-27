using Abp.Application.Services;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using MHPQ.ApbCore.Data;
using MHPQ.Common.DataResult;
using MHPQ.EntityDb;
using MHPQ.Services.Dto;
using System;
using System.Threading.Tasks;

namespace MHPQ.Services
{
    public interface ITenantUserBusinessAppService : IApplicationService
    {
        #region Items
        Task<object> GetAllSetItemsAsync();
        Task<object> GetAllItemTypeAsync();
        Task<object> CreateOrUpdateSetItemsAsync(SetItemsDto input);
        Task<object> DeleteSetItemsAsync(long id);
        #endregion
        #region Order
        Task<object> GetAllOrderAsync();
        Task<object> CreateOrUpdateOrderAsync(OrderDto input);
        Task<object> DeleteOrderAsync(long id);
        #endregion
        #region Voucher
        Task<object> GetAllVoucherAsync();
        #endregion

        #region Rating
        Task<object> CreateOrUpdateRateAsync(RateDto input);
        Task<object> GetListRateAsync(GetListRateInput input);
        Task<object> DeleteRateAsync(long id);

        #endregion

    }

    public class TenantUserBusinessAppService : MHPQAppServiceBase, ITenantUserBusinessAppService
    {
        private readonly IRepository<ObjectPartner, long> _objectadminRepos;
        private readonly IRepository<ObjectType, long> _objectTypeRepos;
        private readonly IRepository<Items, long> _itemsRepos;
        private readonly IRepository<ItemType, long> _itemTypeRepos;
        private readonly IRepository<BusinessNotify, long> _businessNotiRepos;
        private readonly IRepository<Voucher, long> _voucherRepos;
        private readonly IRepository<Order, long> _orderRepos;
        private readonly IRepository<SetItems, long> _setItemsRepos;
        private readonly IRepository<ItemViewSetting, long> _itemViewSettingRepos;
        private readonly IRepository<Rate, long> _rateRepos;
        private readonly ISqlExecuter _sqlExecute;


        public TenantUserBusinessAppService(
            IRepository<ObjectPartner, long> objectadminRepos,
            IRepository<ObjectType, long> objectTypeRepos,
            IRepository<Items, long> itemsRepos,
            IRepository<ItemType, long> itemTypeRepos,
            IRepository<BusinessNotify, long> businessNotiRepos,
            IRepository<Voucher, long> voucherRepos,
            IRepository<SetItems, long> setItemsRepos,
            IRepository<Order, long> orderRepos,
            IRepository<ItemViewSetting, long> itemViewSettingRepos,
            IRepository<Rate, long> rateRepos,
            ISqlExecuter sqlExecute
            )
        {
            _objectadminRepos = objectadminRepos;
            _objectTypeRepos = objectTypeRepos;
            _itemsRepos = itemsRepos;
            _itemTypeRepos = itemTypeRepos;
            _businessNotiRepos = businessNotiRepos;
            _voucherRepos = voucherRepos;
            _orderRepos = orderRepos;
            _setItemsRepos = setItemsRepos;
            _itemViewSettingRepos = itemViewSettingRepos;
            _rateRepos = rateRepos;
            _sqlExecute = sqlExecute;
        }

        #region Items


        public async Task<object> GetAllSetItemsAsync()
        {
            try
            {
                long t1 = TimeUtils.GetNanoseconds();
                var result = await _setItemsRepos.GetAllListAsync();
                var data = DataResult.ResultSucces(result, "Get success");
                mb.statisticMetris(t1, 0, "gall_item");
                return data;
            }
            catch (Exception ex)
            {
                var data = DataResult.ResultError(ex.ToString(), "Có lỗi");
                Logger.Fatal(ex.Message, ex);
                return data;
            }

        }


        public async Task<object> GetAllItemTypeAsync()
        {
            try
            {
                long t1 = TimeUtils.GetNanoseconds();
                var result = await _itemTypeRepos.GetAllListAsync();
                var data = DataResult.ResultSucces(result, "Get success");
                mb.statisticMetris(t1, 0, "gall_item");
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
        public async Task<object> CreateOrUpdateSetItemsAsync(SetItemsDto input)
        {
            try
            {
                long t1 = TimeUtils.GetNanoseconds();

                input.TenantId = AbpSession.TenantId;
                if (input.Id > 0)
                {
                    //update
                    var updateData = await _setItemsRepos.GetAsync(input.Id);
                    if (updateData != null)
                    {
                        input.MapTo(updateData);

                        //call back
                        await _setItemsRepos.UpdateAsync(updateData);
                    }
                    mb.statisticMetris(t1, 0, "user_ud_setitem");

                    var data = DataResult.ResultSucces(updateData, "Update success !");
                    return data;
                }
                else
                {
                    //Insert
                    var insertInput = input.MapTo<SetItems>();
                    long id = await _setItemsRepos.InsertAndGetIdAsync(insertInput);

                    mb.statisticMetris(t1, 0, "user_is_objtype");
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


        public async Task<object> DeleteSetItemsAsync(long id)
        {
            try
            {
                long t1 = TimeUtils.GetNanoseconds();
                await _setItemsRepos.DeleteAsync(id);
                var data = DataResult.ResultSucces("Delete Success");
                mb.statisticMetris(t1, 0, "user_del_setitem");
                return data;
            }
            catch (Exception ex)
            {
                var data = DataResult.ResultError(ex.ToString(), "Có lỗi");
                Logger.Fatal(ex.Message, ex);
                return data;
            }
        }


        #endregion

        #region Voucher
        public async Task<object> GetAllVoucherAsync()
        {
            try
            {
                long t1 = TimeUtils.GetNanoseconds();
                var result = await _voucherRepos.GetAllListAsync();
                var data = DataResult.ResultSucces(result, "Get success");
                mb.statisticMetris(t1, 0, "user_gall_voucher");
                return data;
            }
            catch (Exception ex)
            {
                var data = DataResult.ResultError(ex.ToString(), "Có lỗi");
                Logger.Fatal(ex.Message, ex);
                return data;
            }

        }
        #endregion
        #region Order

        public async Task<object> GetAllOrderAsync()
        {
            try
            {
                long t1 = TimeUtils.GetNanoseconds();
                var result = await _orderRepos.GetAllListAsync();
                var data = DataResult.ResultSucces(result, "Get success");
                mb.statisticMetris(t1, 0, "gall_voucher");
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
        public async Task<object> CreateOrUpdateOrderAsync(OrderDto input)
        {
            try
            {
                long t1 = TimeUtils.GetNanoseconds();

                input.TenantId = AbpSession.TenantId;
                if (input.Id > 0)
                {
                    //update
                    var updateData = await _orderRepos.GetAsync(input.Id);
                    if (updateData != null)
                    {
                        input.MapTo(updateData);

                        //call back
                        await _orderRepos.UpdateAsync(updateData);
                    }
                    mb.statisticMetris(t1, 0, "user_ud_order");

                    var data = DataResult.ResultSucces(updateData, "Update success !");
                    return data;
                }
                else
                {
                    //Insert
                    var insertInput = input.MapTo<Order>();
                    long id = await _orderRepos.InsertAndGetIdAsync(insertInput);

                    mb.statisticMetris(t1, 0, "user_is_order");
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

        public async Task<object> DeleteOrderAsync(long id)
        {
            try
            {
                long t1 = TimeUtils.GetNanoseconds();
                await _orderRepos.DeleteAsync(id);
                var data = DataResult.ResultSucces("Delete Success");
                mb.statisticMetris(t1, 0, "user_del_order");
                return data;
            }
            catch (Exception ex)
            {
                var data = DataResult.ResultError(ex.ToString(), "Có lỗi");
                Logger.Fatal(ex.Message, ex);
                return data;
            }
        }


        #endregion

        #region Rate
        [Obsolete]
        public async Task<object> CreateOrUpdateRateAsync(RateDto input)
        {
            try
            {
                long t1 = TimeUtils.GetNanoseconds();
                input.TenantId = AbpSession.TenantId;
                if (input.Id > 0)
                {
                    //update
                    var updateData = await _rateRepos.GetAsync(input.Id);
                    if (updateData != null)
                    {
                        input.MapTo(updateData);

                        //call back
                        await _rateRepos.UpdateAsync(updateData);
                    }
                    mb.statisticMetris(t1, 0, "user_ud_rate");
                    var data = DataResult.ResultSucces(updateData, "Update success !");
                    return data;
                }
                else
                {
                    //Insert
                    var insertInput = input.MapTo<Rate>();
                    long id = await _rateRepos.InsertAndGetIdAsync(insertInput);

                    mb.statisticMetris(t1, 0, "user_is_rate");
                    var data = DataResult.ResultSucces(insertInput, "Insert success !");
                    return data;
                }


            }
            catch (Exception e)
            {
                var data = DataResult.ResultError(e.ToString(), "Exception !");
                Logger.Fatal(e.Message);
                return data;
            }
        }

        public async Task<object> GetListRateAsync(GetListRateInput input)
        {
            try
            {
                long t1 = TimeUtils.GetNanoseconds();
                var result = await _rateRepos.GetAllListAsync();
                var data = DataResult.ResultSucces(result, "Get success");
                mb.statisticMetris(t1, 0, "gall_rate");
                return data;
            }
            catch (Exception ex)
            {
                var data = DataResult.ResultError(ex.ToString(), "Exception");
                Logger.Fatal(ex.Message, ex);
                return data;
            }
        }

        public async Task<object> DeleteRateAsync(long id)
        {
            try
            {
                long t1 = TimeUtils.GetNanoseconds();
                await _orderRepos.DeleteAsync(id);
                var data = DataResult.ResultSucces("Delete Success");
                mb.statisticMetris(t1, 0, "user_del_order");
                return data;
            }
            catch (Exception ex)
            {
                var data = DataResult.ResultError(ex.ToString(), "Có lỗi");
                Logger.Fatal(ex.Message, ex);
                return data;
            }
        }
        #endregion

    }
}
