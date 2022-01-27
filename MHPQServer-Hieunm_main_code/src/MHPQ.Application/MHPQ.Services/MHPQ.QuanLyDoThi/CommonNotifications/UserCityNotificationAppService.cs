using Abp;
using Abp.Application.Services;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.MultiTenancy;
using Abp.RealTime;
using MHPQ.Authorization.Users;
using MHPQ.Chat;
using MHPQ.Common.DataResult;
using MHPQ.EntityDb;
using MHPQ.Notifications;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHPQ.Services
{
    public interface IUserCityNotificationAppService : IApplicationService
    {
        Task<object> GetAllNotificationUserTenant();
        Task<object> CreatOrUpdateCommentAsync(CommentDto input);
        Task<object> GetNotificationUserOffline();
        Task<object> GetAllFeedbackUser();
        Task<object> CreateOrUpdateFeedback(UserFeedbackDto input);
        Task<object> DeleteFeedback(long id);
        Task<object> GetAllCommnetByFeedback(long id);
        Task<object> CreateOrUpdateFeedbackComment(UserFeedbackCommentDto input);
        Task<object> DeleteFeedbackComment(long id);
    }
    public class UserCityNotificationAppService: MHPQAppServiceBase, IUserCityNotificationAppService
    {
        private readonly IRepository<CityNotification, long> _cityNotificationRepos;
        private readonly IRepository<CityNotificationComment, long> _commentRepos;
        private readonly IRepository<UserFeedback, long> _userFeedbackRepos;
        private readonly IRepository<UserFeedbackComment, long> _userFeedbackCommentRepos;
        private readonly IOnlineClientManager _onlineClientManager;
        private readonly INotificationCommunicator _notificationCommunicator;
        private readonly IChatCommunicator _chatCommunicator;
        private readonly ITenantCache _tenantCache;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IRepository<Citizen, long> _citizenRepos;
        private readonly UserManager _userManager;
        private readonly UserStore _store;
        public UserCityNotificationAppService(
            IRepository<CityNotification, long> cityNotificationRepos,
            IRepository<CityNotificationComment, long> commentRepos,
            IRepository<Citizen, long> citizenRepos,
            IRepository<UserFeedbackComment, long> userFeedbackCommentRepos,
            IChatCommunicator chatCommunicator,
            IOnlineClientManager onlineClientManager,
            INotificationCommunicator notificationCommunicator,
            ITenantCache tenantCache,
            IUnitOfWorkManager unitOfWorkManager,
            IRepository<UserFeedback, long> userFeedbackRepos,
            UserManager userManager,
            UserStore store
            )
        {
            _cityNotificationRepos = cityNotificationRepos;
            _notificationCommunicator = notificationCommunicator;
            _onlineClientManager = onlineClientManager;
            _userFeedbackCommentRepos = userFeedbackCommentRepos;
            _tenantCache = tenantCache;
            _commentRepos = commentRepos;
            _unitOfWorkManager = unitOfWorkManager;
            _userManager = userManager;
            _citizenRepos = citizenRepos;
            _chatCommunicator = chatCommunicator;
            _userFeedbackRepos = userFeedbackRepos;
            _store = store;
        }

        [Obsolete]
        public async Task<object> CreateOrUpdateFeedback(UserFeedbackDto input)
        {
            try
            {
                long t1 = TimeUtils.GetNanoseconds();

                input.TenantId = AbpSession.TenantId;
                if (input.Id > 0)
                {
                    //update
                    var updateData = await _userFeedbackRepos.GetAsync(input.Id);
                    if (updateData != null)
                    {
                        input.MapTo(updateData);
                        //call back
                        await _userFeedbackRepos.UpdateAsync(updateData);
                    }
                    mb.statisticMetris(t1, 0, "Ud_feedback");

                    var data = DataResult.ResultSucces(updateData, "Update success !");
                    return data;
                }
                else
                {

                    var insertInput = input.MapTo<UserFeedback>();
                    long id = await _userFeedbackRepos.InsertAndGetIdAsync(insertInput);
                    insertInput.Id = id;

                    var admins = await _store.GetAllCitizenManagerTenantAsync();
                    //var clients = _onlineClientManager.GetAllClients()
                    //    .Where(c => c.TenantId == AbpSession.TenantId)
                    //    .ToImmutableList();

                    if (admins.Any())
                    {
                        _notificationCommunicator.SendNotificationToAdminTenant(admins, insertInput);
                    }

                    mb.statisticMetris(t1, 0, "is_noti");
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

        [Obsolete]
        public async Task<object> CreateOrUpdateFeedbackComment(UserFeedbackCommentDto input)
        {
            try
            {
                long t1 = TimeUtils.GetNanoseconds();

                input.TenantId = AbpSession.TenantId;
                if (input.Id > 0)
                {
                    //update
                    var updateData = await _userFeedbackCommentRepos.GetAsync(input.Id);
                    if (updateData != null)
                    {
                        input.MapTo(updateData);
                        await _userFeedbackCommentRepos.UpdateAsync(updateData);
                    }
                    mb.statisticMetris(t1, 0, "Ud_fbcomment");

                    var data = DataResult.ResultSucces(updateData, "Update success !");
                    return data;
                }
                else
                {

                    var insertInput = input.MapTo<UserFeedbackComment>();
                    long id = await _userFeedbackCommentRepos.InsertAndGetIdAsync(insertInput);
                    insertInput.Id = id;
                    var admins = await _store.GetAllCitizenManagerTenantAsync();

                    if (admins.Any())
                    {
                        _notificationCommunicator.SendCommentFeedbackToAdminTenant(admins, insertInput);
                    }

                    mb.statisticMetris(t1, 0, "is_noti");
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

        [Obsolete]
        public async Task<object> CreatOrUpdateCommentAsync(CommentDto input)
        {
            try
            {
                long t1 = TimeUtils.GetNanoseconds();

                input.TenantId = AbpSession.TenantId;
                if (input.Id > 0)
                {
                    //update
                    var updateData = await _commentRepos.GetAsync(input.Id);
                    if (updateData != null)
                    {
                        input.MapTo(updateData);
                        //call back
                        await _commentRepos.UpdateAsync(updateData);
                    }
                    mb.statisticMetris(t1, 0, "Ud_comment");

                    var data = DataResult.ResultSucces(updateData, "Update success !");
                    return data;
                }
                else
                {

                    var insertInput = input.MapTo<CityNotificationComment>();
                    long id = await _commentRepos.InsertAndGetIdAsync(insertInput);
                    insertInput.Id = id;
                    mb.statisticMetris(t1, 0, "is_comment");
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

        public async Task<object> DeleteFeedback(long id)
        {
            try
            {

                await _userFeedbackRepos.DeleteAsync(id);
                var data = DataResult.ResultSucces("Delete success!");
                return data;
            }
            catch (Exception e)
            {
                var data = DataResult.ResultError(e.ToString(), "Exception !");
                Logger.Fatal(e.Message);
                return data;
            }
        }

        public async Task<object> DeleteFeedbackComment(long id)
        {
            try
            {

                await _userFeedbackCommentRepos.DeleteAsync(id);
                var data = DataResult.ResultSucces("Delete success!");
                return data;
            }
            catch (Exception e)
            {
                var data = DataResult.ResultError(e.ToString(), "Exception !");
                Logger.Fatal(e.Message);
                return data;
            }
        }

        public async Task<object> GetAllCommnetByFeedback(long id)
        {
            try
            {

                var result = await _userFeedbackCommentRepos.GetAll()
                    .Where(x => x.FeedbackId == id)
                    .OrderByDescending(m => m.CreationTime)
                    .Take(20)
                    .ToListAsync();
                var data = DataResult.ResultSucces(result, "Get success!");
                return data;
            }
            catch (Exception e)
            {
                var data = DataResult.ResultError(e.ToString(), "Exception !");
                Logger.Fatal(e.Message);
                return data;
            }
        }

        public async Task<object> GetAllFeedbackUser()
        {
            try
            {

                var result = await _userFeedbackRepos.GetAllListAsync(x => x.CreatorUserId == AbpSession.UserId);
                var data = DataResult.ResultSucces(result, "Get success!");
                return data;
            }
            catch (Exception e)
            {
                var data = DataResult.ResultError(e.ToString(), "Exception !");
                Logger.Fatal(e.Message);
                return data;
            }
        }

        public async Task<object> GetAllNotificationUserTenant()
        {
            try
            {

                var result = await _cityNotificationRepos.GetAllListAsync();
                var data = DataResult.ResultSucces(result, "Get success!");
                return data;
            }
            catch (Exception e)
            {
                var data = DataResult.ResultError(e.ToString(), "Exception !");
                Logger.Fatal(e.Message);
                return data;
            }
        }

        public async Task<object> GetNotificationUserOffline()
        {
            try
            {

                var result = await _cityNotificationRepos.GetAllListAsync();
                var data = DataResult.ResultSucces(result, "Get success!");
                return data;
            }
            catch (Exception e)
            {
                var data = DataResult.ResultError(e.ToString(), "Exception !");
                Logger.Fatal(e.Message);
                return data;
            }
        }
    }
}
