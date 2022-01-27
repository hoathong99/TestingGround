using Abp.Application.Services;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using MHPQ.ApbCore.Data;
using MHPQ.Common.DataResult;
using MHPQ.EntityDb;
using MHPQ.Services.Dto;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MHPQ.Services
{
    public interface ISuperAdminBusinessAppService : IApplicationService
    {
        #region Object
        Task<object> GetAllObjectAsync(ObjectInputDto input);
        Task<object> CreateListObjectAsync(List<ObjectDto> input);
        Task<object> GetObjectByTypeAsync(int type);
        Task<object> CreateOrUpdateObject(ObjectDto input);
        Task<object> DeleteObject(long id);

        Task<object> UpdateStateObject(List<long> ids, int state);
        Task<object> DeleteListObject(List<long> ids);
        #endregion
        #region TypeObject
        Task<object> GetAllTypeObjectAsync();
        Task<object> CreateOrUpdateTypeObject(ObjectTypeDto input);
        Task<object> DeleteTypeObject(long id);
        #endregion
        #region Items
        Task<object> GetAllItemsAsync(ItemsInputDto input);
        Task<object> GetAllSetItemsAsync();
        Task<object> GetAllItemTypeAsync(ItemTypeInputDto input);
        Task<object> CreateOrUpdateItemsAsync(ItemsDto input);
        Task<object> CreateOrUpdateSetItemsAsync(SetItemsDto input);
        Task<object> CreateOrUpdateTypeItemAsync(ItemTypeDto input);

        Task<object> UpdateStateListItemsAsync(List<long> ids, int state);
        Task<object> GetAllItemViewSettingAsync(ItemViewSettingInputDto input);
        Task<object> GetItemViewSettingsAsync(ItemViewSettingInputDto input);
        Task<object> CreateOrUpdateItemViewSettingAsync(ItemViewSettingDto input);
        Task<object> DeleteItemViewSettingAsync(long id);
        Task<object> DeleteItemsAsync(long id);
        Task<object> DeleteSetItemsAsync(long id);
        Task<object> DeleteItemTypeAsync(long id);
        #endregion
        #region Order
        Task<object> GetAllOrderAsync();
        Task<object> CreateOrUpdateOrderAsync(OrderDto input);
        Task<object> DeleteOrderAsync(long id);
        #endregion
        #region Voucher
        Task<object> GetAllVoucherAsync();
        Task<object> CreateOrUpdateVoucherAsync(VoucherDto input);
        Task<object> DeleteVoucherAsync(long id);
        #endregion
    }

    public class SuperAdminBusinessAppService : MHPQAppServiceBase, ISuperAdminBusinessAppService
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
       

        public SuperAdminBusinessAppService(
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


        #region Object
        public async Task<object> GetAllObjectAsync(ObjectInputDto input)
        {
            try
            {
                long t1 = TimeUtils.GetNanoseconds();
                var result = await _objectadminRepos.GetAllListAsync();
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

        public async Task<object> GetObjectByTypeAsync(int type)
        {
            try
            {
                long t1 = TimeUtils.GetNanoseconds();

                var result = await _objectadminRepos.FirstOrDefaultAsync(x => x.Type == type && x.CreatorUserId == AbpSession.UserId);
                var data = DataResult.ResultSucces(result, "Get success");
                mb.statisticMetris(t1, 0, "admin_gallbytype_obj");
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
        public async Task<object> CreateListObjectAsync(List<ObjectDto> input)
        {
            try
            {
                ConcurrentDictionary<int, Task<long>> concurrentTasks = new ConcurrentDictionary<int, Task<long>>();
                ConcurrentDictionary<int, Task<long>> taskRates = new ConcurrentDictionary<int, Task<long>>();
                long t1 = TimeUtils.GetNanoseconds();
                
                if(input != null)
                {
                    var index = 0;
                    foreach ( var obj in input)
                    {
                        index++;
                        if(obj.Name != null)
                        {
                            
                            var insertInput = obj.MapTo<ObjectPartner>();
                            insertInput.TenantId = null;
                            await _objectadminRepos.InsertAsync(insertInput);

                        }
                    }
                  
                }
             
                await CurrentUnitOfWork.SaveChangesAsync();
                
                mb.statisticMetris(t1, 0, "admin_islist_obj");
                var data = DataResult.ResultSucces("Insert success !");
                return data;

            }
            catch (Exception e)
            {
                var data = DataResult.ResultError(e.ToString(), "Exception");
                Logger.Fatal(e.Message);
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
                    var updateData = await _objectadminRepos.GetAsync(input.Id);
                    if (updateData != null)
                    {
                        input.MapTo(updateData);
                        //call back
                        await _objectadminRepos.UpdateAsync(updateData);
                    }
                    mb.statisticMetris(t1, 0, "admin_ud_obj");
                    var data = DataResult.ResultSucces(updateData, "Insert success !");
                    return data;
                }
                else
                {
                    //Insert
                    var insertInput = input.MapTo<ObjectPartner>();
                    long id = await _objectadminRepos.InsertAndGetIdAsync(insertInput);

                    mb.statisticMetris(t1, 0, "admin_is_obj");
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
                await _objectadminRepos.DeleteAsync(id);
                var data = DataResult.ResultSucces("Delete Success");
                mb.statisticMetris(t1, 0, "admin_del_obj");
                return data;
            }
            catch (Exception ex)
            {
                var data = DataResult.ResultError(ex.ToString(), "Có lỗi");
                Logger.Fatal(ex.Message, ex);
                return data;
            }
        }

        public async Task<object> UpdateStateObject(List<long> ids, int state)
        {
            try
            {
                long t1 = TimeUtils.GetNanoseconds();
                StringBuilder sb = new StringBuilder();

                foreach (long id in ids)
                {
                    sb.AppendFormat("{0},", id);
                }

                var sql = string.Format("UPDATE ObjectPartners" +
                    "SET State = {1}, LastModificationTime = CURRENT_TIMESTAMP, DeleterUserId = {2}" +
                    " WHERE Id IN ({0})",
                    sb.ToString().TrimEnd(','), state, AbpSession.UserId);
                var par = new SqlParameter();
                //await _objectadminRepos.DeleteManyAsync(x => x.)
                var i = await _sqlExecute.ExecuteAsync(sql);
                CurrentUnitOfWork.SaveChanges();
                var data = DataResult.ResultSucces("Update state Success");
                mb.statisticMetris(t1, 0, "admin_udstateobj_obj");
                return data;
            }
            catch (Exception ex)
            {
                var data = DataResult.ResultError(ex.ToString(), "Có lỗi");
                Logger.Fatal(ex.Message, ex);
                return data;
            }
        }

        public async Task<object> DeleteListObject(List<long> ids)
        {
            try
            {
                long t1 = TimeUtils.GetNanoseconds();
                StringBuilder sb = new StringBuilder();
               
                foreach (long id in ids)
                {
                    sb.AppendFormat("{0},", id);
                }

                var sql = string.Format("UPDATE ObjectPartners" +
                    " SET IsDelete = 1,  DeleterUserId =  {1}, DeletionTime = CURRENT_TIMESTAMP " +
                    " WHERE Id IN ({0})",
                    sb.ToString().TrimEnd(','),
                    AbpSession.UserId
                    );
                var par = new SqlParameter();
                //await _objectadminRepos.DeleteManyAsync(x => x.)
                var i = await _sqlExecute.ExecuteAsync(sql);
                CurrentUnitOfWork.SaveChanges();
                var data = DataResult.ResultSucces("Delete Success");
                mb.statisticMetris(t1, 0, "admin_del_obj");
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

        [Obsolete]
        public async Task<object> CreateOrUpdateTypeObject(ObjectTypeDto input)
        {
            try
            {
                long t1 = TimeUtils.GetNanoseconds();

                input.TenantId = null;
                if (input.Id > 0)
                {
                    //update
                    var updateData = await _objectTypeRepos.GetAsync(input.Id);
                    if (updateData != null)
                    {
                        input.MapTo(updateData);

                        //call back
                        await _objectTypeRepos.UpdateAsync(updateData);
                    }
                    mb.statisticMetris(t1, 0, "admin_ud_objtype");

                    var data = DataResult.ResultSucces(updateData, "Update success !");
                    return data;
                }
                else
                {
                    //Insert
                    var insertInput = input.MapTo<ObjectType>();
                    long id = await _objectTypeRepos.InsertAndGetIdAsync(insertInput);

                    mb.statisticMetris(t1, 0, "admin_is_objtype");
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

        public async Task<object> DeleteTypeObject(long id)
        {
            try
            {
                long t1 = TimeUtils.GetNanoseconds();
                await _objectTypeRepos.DeleteAsync(id);
                var data = DataResult.ResultSucces("Delete Success");
                mb.statisticMetris(t1, 0, "admin_del_obj");
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
        public async Task<object> GetAllItemsAsync(ItemsInputDto input)
        {
            try
            {
                long t1 = TimeUtils.GetNanoseconds();
                var result = await _itemsRepos.GetAllListAsync(x => (input.Id != null && x.Id == input.Id) || (input.Type != null && x.Type == input.Type) || (input.Keyword != null && x.QueryKey.Contains(input.Keyword)));
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


        public async Task<object> GetAllItemTypeAsync(ItemTypeInputDto input)
        {
            try
            {
                long t1 = TimeUtils.GetNanoseconds();
                var result = await _itemTypeRepos.GetAllListAsync(x => (input.Id != null && x.Id == input.Id) || (input.Type != null && x.Type == input.Type) || (input.Keyword != null && x.QueryKey.Contains(input.Keyword)));
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


        public async Task<object> GetAllItemViewSettingAsync(ItemViewSettingInputDto input)
        {
            try
            {
                long t1 = TimeUtils.GetNanoseconds();
                var result = await _itemViewSettingRepos.FirstOrDefaultAsync(x => (input.Id != null && x.Id == input.Id) || (input.Type != null && x.Type == input.Type) || (input.Keyword != null && x.QueryKey.Contains(input.Keyword)));
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

        public async Task<object> GetItemViewSettingsAsync(ItemViewSettingInputDto input)
        {
            try
            {
                long t1 = TimeUtils.GetNanoseconds();
                var result = await _itemViewSettingRepos.GetAllListAsync(x => (input.Id != null && x.Id == input.Id) || (input.Type != null && x.Type == input.Type) || (input.Keyword != null && x.QueryKey.Contains(input.Keyword)));
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
                    mb.statisticMetris(t1, 0, "admin_ud_item");

                    var data = DataResult.ResultSucces(updateData, "Update success !");
                    return data;
                }
                else
                {
                    //Insert
                    var insertInput = input.MapTo<Items>();
                    long id = await _itemsRepos.InsertAndGetIdAsync(insertInput);

                    mb.statisticMetris(t1, 0, "admin_is_item");
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
        public async Task<object> CreateOrUpdateTypeItemAsync(ItemTypeDto input)
        {
            try
            {
                long t1 = TimeUtils.GetNanoseconds();


                if (input.Id > 0)
                {
                    //update
                    var updateData = await _itemTypeRepos.GetAsync(input.Id);
                    if (updateData != null)
                    {
                        input.MapTo(updateData);

                        //call back
                        await _itemTypeRepos.UpdateAsync(updateData);
                    }
                    mb.statisticMetris(t1, 0, "admin_ud_itemtype");

                    var data = DataResult.ResultSucces(updateData, "Update success !");
                    return data;
                }
                else
                {
                    //Insert
                    var insertInput = input.MapTo<ItemTypeDto>();
                    long id = await _itemTypeRepos.InsertAndGetIdAsync(insertInput);
                    if(id > 0)
                    {
                        insertInput.Id = id;
                    }
                    mb.statisticMetris(t1, 0, "admin_is_itemtype");
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
                    mb.statisticMetris(t1, 0, "admin_ud_setitem");

                    var data = DataResult.ResultSucces(updateData, "Update success !");
                    return data;
                }
                else
                {
                    //Insert
                    var insertInput = input.MapTo<SetItems>();
                    long id = await _setItemsRepos.InsertAndGetIdAsync(insertInput);

                    mb.statisticMetris(t1, 0, "admin_is_setitem");
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
        public async Task<object> CreateOrUpdateItemViewSettingAsync(ItemViewSettingDto input)
        {
            try
            {
                long t1 = TimeUtils.GetNanoseconds();

                input.TenantId = null;
                if (input.Id > 0)
                {
                    //update
                    var updateData = await _itemViewSettingRepos.GetAsync(input.Id);
                    if (updateData != null)
                    {
                        input.MapTo(updateData);

                        //call back
                        await _itemViewSettingRepos.UpdateAsync(updateData);
                    }
                    mb.statisticMetris(t1, 0, "admin_ud_itemtype");

                    var data = DataResult.ResultSucces(updateData, "Update success !");
                    return data;
                }
                else
                {
                    //Insert
                    var insertInput = input.MapTo<ItemViewSetting>();
                    long id = await _itemViewSettingRepos.InsertAndGetIdAsync(insertInput);
                    if (id > 0)
                    {
                        insertInput.Id = id;
                    }
                    mb.statisticMetris(t1, 0, "admin_is_itemtview");
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

        public async Task<object> UpdateStateListItemsAsync(List<long> ids, int state)
        {
            try
            {
                long t1 = TimeUtils.GetNanoseconds();
                StringBuilder sb = new StringBuilder();

                foreach (long id in ids)
                {
                    sb.AppendFormat("{0},", id);
                }

                var sql = string.Format("UPDATE Items " +
                    "SET State = {1}, LastModificationTime = CURRENT_TIMESTAMP, DeleterUserId = {2}" +
                    " WHERE Id IN ({0})", 
                    sb.ToString().TrimEnd(','), state, AbpSession.UserId);
                //var par = new SqlParameter();
                //await _objectadminRepos.DeleteManyAsync(x => x.)
                var i = await _sqlExecute.ExecuteAsync(sql);
                CurrentUnitOfWork.SaveChanges();
                var data = DataResult.ResultSucces("Update state item Success");
                mb.statisticMetris(t1, 0, "admin_udstateitem_obj");
                return data;
            }
            catch (Exception ex)
            {
                var data = DataResult.ResultError(ex.ToString(), "Có lỗi");
                Logger.Fatal(ex.Message, ex);
                return data;
            }
        }
        public async Task<object> DeleteItemViewSettingAsync(long id)
        {
            try
            {
                long t1 = TimeUtils.GetNanoseconds();
                await _itemViewSettingRepos.DeleteAsync(id);
                var data = DataResult.ResultSucces("Delete Success");
                mb.statisticMetris(t1, 0, "admin_del_itemview");
                return data;
            }
            catch (Exception ex)
            {
                var data = DataResult.ResultError(ex.ToString(), "Có lỗi");
                Logger.Fatal(ex.Message, ex);
                return data;
            }
        }

        public async Task<object> DeleteItemsAsync(long id)
        {
            try
            {
                long t1 = TimeUtils.GetNanoseconds();
                await _itemsRepos.DeleteAsync(id);
                var data = DataResult.ResultSucces("Delete Success");
                mb.statisticMetris(t1, 0, "admin_del_item");
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
                var data = DataResult.ResultSucces("Delete Success");
                mb.statisticMetris(t1, 0, "admin_del_setitem");
                return data;
            }
            catch (Exception ex)
            {
                var data = DataResult.ResultError(ex.ToString(), "Có lỗi");
                Logger.Fatal(ex.Message, ex);
                return data;
            }
        }

        public async Task<object> DeleteItemTypeAsync(long id)
        {
            try
            {
                long t1 = TimeUtils.GetNanoseconds();
                await _itemTypeRepos.DeleteAsync(id);
                var data = DataResult.ResultSucces("Delete Success");
                mb.statisticMetris(t1, 0, "admin_del_setitem");
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
                mb.statisticMetris(t1, 0, "admin_gall_voucher");
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
                    mb.statisticMetris(t1, 0, "admin_ud_order");

                    var data = DataResult.ResultSucces(updateData, "Update success !");
                    return data;
                }
                else
                {
                    //Insert
                    var insertInput = input.MapTo<Order>();
                    long id = await _orderRepos.InsertAndGetIdAsync(insertInput);

                    mb.statisticMetris(t1, 0, "admin_is_order");
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
                var data = DataResult.ResultSucces( "Delete Success");
                mb.statisticMetris(t1, 0, "admin_del_order");
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
                    mb.statisticMetris(t1, 0, "admin_ud_order");

                    var data = DataResult.ResultSucces(updateData, "Update success !");
                    return data;
                }
                else
                {
                    //Insert
                    var insertInput = input.MapTo<Order>();
                    long id = await _orderRepos.InsertAndGetIdAsync(insertInput);

                    mb.statisticMetris(t1, 0, "admin_is_order");
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
                mb.statisticMetris(t1, 0, "admin_del_order");
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
