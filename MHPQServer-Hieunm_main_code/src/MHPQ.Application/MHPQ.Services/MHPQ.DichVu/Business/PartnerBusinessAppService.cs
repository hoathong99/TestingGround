using Abp.Application.Services;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using MHPQ.Common.DataResult;
using MHPQ.EntityDb;
using MHPQ.Services.Dto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHPQ.MHPQ.Services.MHPQ.DichVu.Business
{
    public interface IPartnerBusinessAppService : IApplicationService
    {
        #region Object
        Task<object> GetObjectDetailAsync();
        Task<object> CreateOrUpdateObject(ObjectDto input);
        Task<object> DeleteObject(long id);
        #endregion
        #region TypeObject
        Task<object> GetAllTypeObjectAsync();
        #endregion
        #region Items
        Task<object> GetAllItemsAsync();
        Task<object> GetAllSetItemsAsync();
        Task<object> GetAllItemTypeAsync();
        Task<object> CreateOrUpdateItemsAsync(ItemsDto input);
        Task<object> CreateOrUpdateSetItemsAsync(SetItemsDto input);
        Task<object> DeleteItemsAsync(long id);
        Task<object> DeleteSetItemsAsync(long id);
        Task<object> GetAllItemViewSettingAsync();
        #endregion
        #region Order
        Task<object> GetAllOrderAsync(OrderFilterDto input);
        //Task<object> CreateOrUpdateOrderAsync(OrderDto input);
        //Task<object> DeleteOrderAsync(long id);
        #endregion
        #region Voucher
        Task<object> GetAllVoucherAsync();
        Task<object> CreateOrUpdateVoucherAsync(VoucherDto input);
        Task<object> DeleteVoucherAsync(long id);
        #endregion

        #region Rating
        Task<object> CreateOrUpdateRateAsync(RateDto input);
        Task<object> GetListRateAsync(GetListRateInput input);
        Task<object> DeleteRateAsync(long id);

        #endregion

    }

    public class PartnerBusinessAppService: MHPQAppServiceBase, IPartnerBusinessAppService
    {

        private readonly IRepository<ObjectPartner, long> _objectPartnerRepos;
        private readonly IRepository<ObjectType, long> _objectTypeRepos;
        private readonly IRepository<Items, long> _itemsRepos;
        private readonly IRepository<ItemType, long> _itemTypeRepos;
        private readonly IRepository<BusinessNotify, long> _businessNotiRepos;
        private readonly IRepository<Voucher, long> _voucherRepos;
        private readonly IRepository<Order, long> _orderRepos;
        private readonly IRepository<SetItems, long> _setItemsRepos;
        private readonly IRepository<ItemViewSetting, long> _itemViewSettingRepos;
        private readonly IRepository<Rate, long> _rateRepos;
        public PartnerBusinessAppService(
            IRepository<ObjectPartner, long> objectPartnerRepos,
            IRepository<ObjectType, long> objectTypeRepos,
            IRepository<Items, long> itemsRepos,
            IRepository<ItemType, long> itemTypeRepos,
            IRepository<BusinessNotify, long> businessNotiRepos,
            IRepository<Voucher, long> voucherRepos,
            IRepository<SetItems, long> setItemsRepos,
            IRepository<Order, long> orderRepos,
            IRepository<ItemViewSetting, long> itemViewSettingRepos,
            IRepository<Rate, long> rateRepos
            )
        {
            _objectPartnerRepos = objectPartnerRepos;
            _objectTypeRepos = objectTypeRepos;
            _itemsRepos = itemsRepos;
            _itemTypeRepos = itemTypeRepos;
            _businessNotiRepos = businessNotiRepos;
            _voucherRepos = voucherRepos;
            _orderRepos = orderRepos;
            _setItemsRepos = setItemsRepos;
            _itemViewSettingRepos = itemViewSettingRepos;
            _rateRepos = rateRepos;
        }


        #region Object
        public async Task<object> GetObjectDetailAsync()
        {
            try
            {
                long t1 = TimeUtils.GetNanoseconds();
                var result = await _objectPartnerRepos.FirstOrDefaultAsync(x => x.TenantId == null && x.CreatorUserId == AbpSession.UserId);
                var data = DataResult.ResultSucces(result, "Get success");
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

        [Obsolete]
        public async Task<object> CreateOrUpdateObject(ObjectDto input)
        {
            try
            {
                long t1 = TimeUtils.GetNanoseconds();
                input.TenantId = null;
                if (input.Id > 0)
                {
                    //update
                    var updateData = await _objectPartnerRepos.GetAsync(input.Id);
                    if (updateData != null)
                    {
                        input.MapTo(updateData);

                        //call back
                        await _objectPartnerRepos.UpdateAsync(updateData);
                    }
                    mb.statisticMetris(t1, 0, "partner_ud_obj");
                    var data = DataResult.ResultSucces(updateData, "Insert success !");
                    return data;
                }
                else
                {
                    //Insert
                    var insertInput = input.MapTo<ObjectPartner>();
                    insertInput.TenantId = null;
                    long id = await _objectPartnerRepos.InsertAndGetIdAsync(insertInput);

                    mb.statisticMetris(t1, 0, "partner_is_obj");
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

        public async Task<object> DeleteObject(long id)
        {
            try
            {
                long t1 = TimeUtils.GetNanoseconds();
                await _objectPartnerRepos.DeleteAsync(id);
                var data = DataResult.ResultSucces( "Delete Success");
                mb.statisticMetris(t1, 0, "partner_del_obj");
                return data;
            }
            catch (Exception ex)
            {
                var data = DataResult.ResultError(ex.ToString(), "Có lỗi");
                Logger.Fatal(ex.Message, ex);
                return data;
            }
        }

        #region Type Object

        public async Task<object> GetAllTypeObjectAsync()
        {
            try
            {
                long t1 = TimeUtils.GetNanoseconds();
                var result = await _objectTypeRepos.GetAllListAsync();
                var data = DataResult.ResultSucces(result, "Get success");
                mb.statisticMetris(t1, 0, "gall_objType");
                return data;
            }
            catch (Exception ex)
            {
                var data = DataResult.ResultError(ex.ToString(), "Có lỗi");
                Logger.Fatal(ex.Message, ex);
                return data;
            }
        }

       
        public async Task<object> DeleteTypeObject(long id)
        {
            try
            {
                long t1 = TimeUtils.GetNanoseconds();
                await _objectTypeRepos.DeleteAsync(id);
                var data = DataResult.ResultSucces("Delete Success");
                mb.statisticMetris(t1, 0, "partner_del_obj");
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
        #endregion

        #region Items
        public async Task<object> GetAllItemsAsync()
        {
            try
            {
                long t1 = TimeUtils.GetNanoseconds();
                var result =  await _itemsRepos.GetAllListAsync(x => x.TenantId == null);
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


        public async Task<object> GetAllSetItemsAsync()
        {
            try
            {
                long t1 = TimeUtils.GetNanoseconds();
                var result = await _setItemsRepos.GetAllListAsync(x => x.TenantId == null);
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
                var result = await _itemTypeRepos.GetAllListAsync(x => x.TenantId == null);
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
        public async Task<object> CreateOrUpdateItemsAsync(ItemsDto input)
        {
            try
            {
                long t1 = TimeUtils.GetNanoseconds();

                input.TenantId = null;
                if (input.Id > 0)
                {
                    //update
                    var updateData = await _itemsRepos.GetAsync(input.Id);
                    if (updateData != null)
                    {
                        input.MapTo(updateData);

                        //call back
                        await _itemsRepos.UpdateAsync(updateData);
                    }
                    mb.statisticMetris(t1, 0, "partner_ud_item");

                    var data = DataResult.ResultSucces(updateData, "Update success !");
                    return data;
                }
                else
                {
                    //Insert
                    var insertInput = input.MapTo<Items>();
                    long id = await _itemsRepos.InsertAndGetIdAsync(insertInput);

                    mb.statisticMetris(t1, 0, "partner_is_item");
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
        public async Task<object> CreateOrUpdateSetItemsAsync(SetItemsDto input)
        {
            try
            {
                long t1 = TimeUtils.GetNanoseconds();

                input.TenantId = null;
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
                    mb.statisticMetris(t1, 0, "partner_ud_setitem");

                    var data = DataResult.ResultSucces(updateData, "Update success !");
                    return data;
                }
                else
                {
                    //Insert
                    var insertInput = input.MapTo<SetItems>();
                    long id = await _setItemsRepos.InsertAndGetIdAsync(insertInput);

                    mb.statisticMetris(t1, 0, "partner_is_objtype");
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

        public async Task<object> DeleteItemsAsync(long id)
        {
            try
            {
                long t1 = TimeUtils.GetNanoseconds();
                 await _itemsRepos.DeleteAsync(id);
                var data = DataResult.ResultSucces( "Delete Success");
                mb.statisticMetris(t1, 0, "partner_del_item");
                return data;
            }
            catch (Exception ex)
            {
                var data = DataResult.ResultError(ex.ToString(), "Có lỗi");
                Logger.Fatal(ex.Message, ex);
                return data;
            }
        }

        public async Task<object> DeleteSetItemsAsync(long id)
        {
            try
            {
                long t1 = TimeUtils.GetNanoseconds();
                await _setItemsRepos.DeleteAsync(id);
                var data = DataResult.ResultSucces( "Delete Success");
                mb.statisticMetris(t1, 0, "partner_del_setitem");
                return data;
            }
            catch (Exception ex)
            {
                var data = DataResult.ResultError(ex.ToString(), "Có lỗi");
                Logger.Fatal(ex.Message, ex);
                return data;
            }
        }

        public async Task<object> GetAllItemViewSettingAsync()
        {
            try
            {
                long t1 = TimeUtils.GetNanoseconds();
                var result = await _itemViewSettingRepos.FirstOrDefaultAsync(x =>x.TenantId == null && x.Type == 1);
                var data = DataResult.ResultSucces(result, "Get success");
                mb.statisticMetris(t1, 0, "partner_gall_itemtype");
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
                var result = await _voucherRepos.GetAllListAsync(x => x.TenantId == null);
                var data = DataResult.ResultSucces(result, "Get success");
                mb.statisticMetris(t1, 0, "partner_gall_voucher");
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

        public async Task<object> GetAllOrderAsync(OrderFilterDto input)
        {
            try
            {
                long t1 = TimeUtils.GetNanoseconds();
                var query = Enumerable.Empty<object>().AsQueryable();
                if(input.State == null)
                {

                    query = (from ord in _orderRepos.GetAll()
                                 join obj in _objectPartnerRepos.GetAll() on ord.ObjectPartnerId equals obj.Id into tb_obj
                                 from obj in tb_obj.DefaultIfEmpty()
                                 where ord.TenantId == null && obj.CreatorUserId == AbpSession.UserId
                                 && ord.Type == input.Type
                                 select new OrderDto()
                                 {
                                     Id = ord.Id,
                                     CreationTime = ord.CreationTime,
                                     OrdererId = ord.OrdererId,
                                     Orderer = ord.Orderer,
                                     Items = ord.Items,
                                     Properties = ord.Properties,
                                     State = ord.State,
                                     Type = ord.Type
                                 }).AsQueryable();
                }
                else
                {
                    query = (from ord in _orderRepos.GetAll()
                                 join obj in _objectPartnerRepos.GetAll() on ord.ObjectPartnerId equals obj.Id into tb_obj
                                 from obj in tb_obj.DefaultIfEmpty()
                                 where obj.CreatorUserId == AbpSession.UserId
                                 && ord.Type == input.Type && ord.State == input.State
                                 select new OrderDto()
                                 {
                                     Id = ord.Id,
                                     CreationTime = ord.CreationTime,
                                     OrdererId = ord.OrdererId,
                                     Orderer = ord.Orderer,
                                     Items = ord.Items,
                                     Properties = ord.Properties,
                                     State = ord.State,
                                     Type = ord.Type
                                 }).AsQueryable();
                }
                var result = await query.Take(20).ToListAsync();
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

                input.TenantId = null;
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
                    mb.statisticMetris(t1, 0, "partner_ud_order");

                    var data = DataResult.ResultSucces(updateData, "Update success !");
                    return data;
                }
                else
                {
                    //Insert
                    var insertInput = input.MapTo<Order>();
                    long id = await _orderRepos.InsertAndGetIdAsync(insertInput);

                    mb.statisticMetris(t1, 0, "partner_is_order");
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
                mb.statisticMetris(t1, 0, "partner_del_order");
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
        public async Task<object> CreateOrUpdateVoucherAsync(VoucherDto input)
        {
            try
            {
                long t1 = TimeUtils.GetNanoseconds();
                  
                input.TenantId = null;
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
                    mb.statisticMetris(t1, 0, "partner_ud_order");

                    var data = DataResult.ResultSucces(updateData, "Update success !");
                    return data;
                }
                else
                {
                    //Insert
                    var insertInput = input.MapTo<Order>();
                    long id = await _orderRepos.InsertAndGetIdAsync(insertInput);

                    mb.statisticMetris(t1, 0, "partner_is_order");
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

        public async Task<object> DeleteVoucherAsync(long id)
        {
            try
            {
                long t1 = TimeUtils.GetNanoseconds();
                await _orderRepos.DeleteAsync(id);
                var data = DataResult.ResultSucces( "Delete Success");
                mb.statisticMetris(t1, 0, "partner_del_order");
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
                input.TenantId = null;
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
                    mb.statisticMetris(t1, 0, "admin_ud_rate");
                    var data = DataResult.ResultSucces(updateData, "Update success !");
                    return data;
                }
                else
                {
                    //Insert
                    var insertInput = input.MapTo<Rate>();
                    long id = await _rateRepos.InsertAndGetIdAsync(insertInput);

                    mb.statisticMetris(t1, 0, "admin_is_rate");
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

        public async Task<object> GetListRateAsync(GetListRateInput input)
        {
            try
            {
                long t1 = TimeUtils.GetNanoseconds();
                var result = await _rateRepos.GetAllListAsync(x => x.TenantId == null);
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
