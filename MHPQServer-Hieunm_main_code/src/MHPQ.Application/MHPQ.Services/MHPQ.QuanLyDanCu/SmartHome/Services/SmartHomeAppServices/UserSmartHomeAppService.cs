using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Abp;
using Abp.Application.Services;
using Abp.Authorization;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.MultiTenancy;
using Abp.RealTime;
using MHPQ.Authorization;
using MHPQ.Authorization.Users;
using MHPQ.Common.DataResult;
using MHPQ.EntityDb;
using MHPQ.Notifications;
using MHPQ.Services.Dto;
using Microsoft.CSharp.RuntimeBinder;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;

namespace MHPQ.Services
{
    public interface IUserSmartHomeAppService : IApplicationService
    {
        Task<object> CreateSmartHomeAsync(CreateSmartHomeInput input);
        Task<object> UpdateSmartHomeAsync(UpdateSmartHomeInput input);
        Task<object> GetSettingSmartHome(string code);
        Task<object> GetByIdSmartHomeAsync(long id);
        Task<object> CreateSmarthomeMember(CreateMemberInput input);
        Task<object> GetAllMembers(string code);
        Task<object> ChangeAdminSmarthome(MemberSmarthomeInput input);
        Task<object> DeleteSmarthome(string code);
        Task<object> DeleteMemberSmarthome(MemberSmarthomeInput input);

    }

    [AbpAuthorize(PermissionNames.Pages_User_Detail)]
    public class UserSmartHomeAppService : MHPQAppServiceBase, IUserSmartHomeAppService
    {
        private readonly IRepository<HomeDevice, long> _homeDeviceRepos;
        private readonly IRepository<HomeGateway, long> _homeGatewayRepos;
        private readonly IRepository<RoomSmartHome, long> _roomSmartHomeRepos;
        private readonly IRepository<SmartHome, long> _smartHomeRepos;
        private readonly IRepository<HouseOwner, long> _houseOwnerRepos;
        private readonly IRepository<User, long> _userRepos;
        private readonly UserManager _userManager;
        private readonly IOnlineClientManager _onlineClientManager;
        private readonly ISmarthomeCommunicator _smarthomeCommunicator;
        private readonly ITenantCache _tenantCache;

        public UserSmartHomeAppService(
            IRepository<RoomSmartHome, long> roomSmartHomeRepos,
            IRepository<HomeDevice, long> homeDeviceRepos,
            IRepository<SmartHome, long> smartHomeRepos,
            IRepository<HomeGateway, long> homeGatewayRepos,
            IRepository<HouseOwner, long> houseOwnerRepos,
            IRepository<User, long> userRepos,
            UserManager userManager,
            IOnlineClientManager onlineClientManager,
            ISmarthomeCommunicator smarthomeCommunicator,
            ITenantCache tenantCache
        )
        {
            _roomSmartHomeRepos = roomSmartHomeRepos;
            _homeDeviceRepos = homeDeviceRepos;
            _smartHomeRepos = smartHomeRepos;
            _homeGatewayRepos = homeGatewayRepos;
            _userManager = userManager;
            _houseOwnerRepos = houseOwnerRepos;
            _onlineClientManager = onlineClientManager;
            _smarthomeCommunicator = smarthomeCommunicator;
            _tenantCache = tenantCache;
            _userRepos = userRepos;
        }


        [Obsolete]
        public async Task<object> CreateSmartHomeAsync(CreateSmartHomeInput input)
        {

            try
            {
                long t1 = TimeUtils.GetNanoseconds();

                var insertInput = new SmartHomeDto();
                insertInput.TenantId = AbpSession.TenantId;
                insertInput.Properties = input.Properties;
                insertInput.SmartHomeCode = GetUniqueKey();
                if (!input.Properties.IsNullOrEmpty())
                {
                    dynamic home = JObject.Parse(input.Properties);
                    try
                    {
                        insertInput.ImageUrl = home.home_infor.img;
                        home.home_infor.home_code = insertInput.SmartHomeCode;
                    }
                    catch (RuntimeBinderException)
                    {
                        var dt = DataResult.ResultCode(null,"Format properties error !", 415);
                        return dt;
                    }
                    insertInput.Properties = Convert.ToString(home);
                }

                long id = await _smartHomeRepos.InsertAndGetIdAsync(insertInput);
                if (id > 0)
                {   
                    insertInput.Id = id;
                    var member = new HouseOwner()
                    {
                        SmartHomeCode = insertInput.SmartHomeCode,
                        IsAdmin = true,
                        UserId = AbpSession.UserId,
                        SmartHomeId = id,
                        TenantId = insertInput.TenantId
                    };

                    await _houseOwnerRepos.InsertAsync(member);
                }
                await CurrentUnitOfWork.SaveChangesAsync();
                var result = new UpdateSmartHomeInput()
                {
                    Properties = insertInput.Properties,
                    SmartHomeCode = insertInput.SmartHomeCode
                };
                mb.statisticMetris(t1, 0, "is_smh");
                var data = DataResult.ResultSucces(result, "Insert success !");
                return data;

            }
            catch (Exception e)
            {
                var data = DataResult.ResultError(e.ToString(), "Có lỗi");
                Logger.Fatal(e.Message);
                return data;
            }
        }

        [Obsolete]
        public async Task<object> UpdateSmartHomeAsync(UpdateSmartHomeInput input)
        {
            try
            {
                long t1 = TimeUtils.GetNanoseconds();
                var updateData = await _smartHomeRepos.FirstOrDefaultAsync(x => (x.TenantId == AbpSession.TenantId) && (x.SmartHomeCode == input.SmartHomeCode));

                if (updateData != null)
                {
                    var oldProp = updateData.Properties;
                    updateData.Properties = input.Properties;
                    updateData.PropertiesHistory = oldProp;
                    await _smartHomeRepos.UpdateAsync(updateData);

                }
                var members = await _houseOwnerRepos.GetAllListAsync(x => (x.TenantId == AbpSession.TenantId) && (x.SmartHomeCode == input.SmartHomeCode) &&(x.UserId != AbpSession.UserId));
                if(members.Count > 0)
                {
                    var clients = new List<IOnlineClient>();
                    foreach(var mb in members)
                    {
                        var user = new UserIdentifier(mb.TenantId, mb.UserId.Value);
                        var client = _onlineClientManager.GetAllByUserId(user);
                        clients.AddRange(client);
                       
                    }
                    _smarthomeCommunicator.NotifyUpdateSmarthomeToClient(clients, input.SmartHomeCode);
                    
                }
                mb.statisticMetris(t1, 0, "mb_up_smh");
                var data = DataResult.ResultSucces(input, "Update success !");
                return data;


            }
            catch (Exception e)
            {
                var data = DataResult.ResultError(e.ToString(), "exception !");
                Logger.Fatal(e.Message, e);
                return data;
            }
        }


     

        [Obsolete]
        public async Task<object> GetByIdSmartHomeAsync(long id)
        {
            try
            {
               
                var smarthome = await _smartHomeRepos.FirstOrDefaultAsync(x => x.Id == id);
                var result = new UpdateSmartHomeInput()
                {
                    Properties = smarthome.Properties,
                    SmartHomeCode = smarthome.SmartHomeCode
                };

                var data = DataResult.ResultSucces(result, "Get success");
                return data;
            }
            catch (Exception e)
            {
                var data = DataResult.ResultError(e.ToString(), "Có lỗi");
                return data;

            }
        }

        [Obsolete]
        public async Task<object> GetSettingSmartHome(string code)
        {
            try
            {
              
                var smarthome = await _smartHomeRepos.FirstOrDefaultAsync(x => x.TenantId == AbpSession.TenantId && (code != null && x.SmartHomeCode == code) );

                if(smarthome != null)
                {
                    var result = new UpdateSmartHomeInput()
                    {
                        Properties = smarthome.Properties,
                        SmartHomeCode = smarthome.SmartHomeCode
                    };
                    var data = DataResult.ResultCode(result, "Get success", 200);
                    return data;
                }
                else
                {
                    var data = DataResult.ResultCode(null, "Smarthome don't exist !", 415);
                    return data;
                }  
            }
            catch (Exception e)
            {
                var data = DataResult.ResultError(e.ToString(), "Exception");
                return data;

            }
        }

        public async Task<object> CreateSmarthomeMember(CreateMemberInput input)
        {
            try
            {
                long t1 = TimeUtils.GetNanoseconds();
                //var isAdmin = AbpSession.UserId;
                var user = await _userManager.FindByNameOrEmailAsync(input.UserNameOrEmail);
                if(user != null)
                {
                    var member = new HouseOwner()
                    {
                        SmartHomeCode = input.SmarthomeCode,
                        UserId = user.Id
                    };
                    await _houseOwnerRepos.InsertAsync(member);
                    mb.statisticMetris(t1, 0, "is_member");
                    var data = DataResult.ResultCode(user, "Add member success !", 200);
                    return data;
                }
                else
                {
                    mb.statisticMetris(t1, 0, "is_member");
                    var data = DataResult.ResultCode(null, "User or email don't exist !", 415);
                    return data;
                }      

            }
            catch (Exception e)
            {
                var data = DataResult.ResultCode(e.ToString(), "Exception !", 500);
                Logger.Fatal(e.Message);
                return data;
            }
        }

        public async Task<object> GetAllMembers(string code)
        {
            try
            {

                var query = (from mb in _houseOwnerRepos.GetAll()
                             join us in _userRepos.GetAll() on mb.UserId equals us.Id
                             where mb.SmartHomeCode == code
                             select new UserSmarthome()
                             {
                                 UserId = us.Id,
                                 FullName = us.FullName,
                                 ImageUrl = us.ImageUrl,
                                 IsAdmin = mb.IsAdmin.Value,
                                 UserName = us.UserName
                             }).AsQueryable();

                var result = await query.ToListAsync();

                var data = DataResult.ResultSucces(result, "Get success");
                return data;
            }
            catch (Exception e)
            {
                var data = DataResult.ResultError(e.ToString(), "Exception !");
                return data;

            }
        }


        public async Task<object> ChangeAdminSmarthome(MemberSmarthomeInput input)
        {
            try
            {

                var admin = await _houseOwnerRepos.GetAsync(AbpSession.UserId.Value);

                if(admin != null && admin.IsAdmin.Value)
                {
                    var newAdmin = await _houseOwnerRepos.GetAsync(input.UserId);
                    if(newAdmin != null)
                    {
                        newAdmin.IsAdmin = true;
                        admin.IsAdmin = false;
                        await _houseOwnerRepos.UpdateAsync(newAdmin);
                        await _houseOwnerRepos.UpdateAsync(admin);
                        await CurrentUnitOfWork.SaveChangesAsync();
                    }
                }

                var data = DataResult.ResultSucces("Update success !");
                return data;
            }
            catch (Exception e)
            {
                var data = DataResult.ResultError(e.ToString(), "Exception !");
                return data;

            }
        }

        public async Task<object> DeleteSmarthome(string code)
        {
            try
            {
                var smarthome = await _smartHomeRepos.FirstOrDefaultAsync(x => x.SmartHomeCode == code);
                var member = await _houseOwnerRepos.FirstOrDefaultAsync(x => x.SmartHomeCode == code && x.UserId == AbpSession.UserId);
                if (smarthome != null && member != null)
                {
                    if(member.IsAdmin.Value)
                    {
                        await _smartHomeRepos.DeleteAsync(smarthome);
                    }
                    else
                    {
                        await _houseOwnerRepos.DeleteAsync(member);
                    }
                    
                    var data = DataResult.ResultSucces("Xóa thành công !");
                    return data;
                }
                else
                {
                    var data = DataResult.ResultFail("Smarthome không tồn tại !");
                    return data;
                }
            }
            catch (Exception e)
            {
                var data = DataResult.ResultError(e.ToString(), "Có lỗi");
                Logger.Fatal(e.Message);
                return data;
            }
        }
        public async Task<object> DeleteMemberSmarthome(MemberSmarthomeInput input)
        {
            try
            {
                var member = await _houseOwnerRepos.FirstOrDefaultAsync(x => x.SmartHomeCode == input.SmarthomeCode && x.UserId == input.UserId);
                var admin = await _houseOwnerRepos.FirstOrDefaultAsync(x => x.SmartHomeCode == input.SmarthomeCode && x.UserId == AbpSession.UserId);
                if (member != null && admin.IsAdmin.Value)
                {
                    await _houseOwnerRepos.DeleteAsync(member);
                    var data = DataResult.ResultSucces("Xóa thành công !");
                    return data;
                }
                else
                {
                    var data = DataResult.ResultFail("Smarthome không tồn tại !");
                    return data;
                }
            }
            catch (Exception e)
            {
                var data = DataResult.ResultError(e.ToString(), "Có lỗi");
                Logger.Fatal(e.Message);
                return data;
            }
        }
    }
}