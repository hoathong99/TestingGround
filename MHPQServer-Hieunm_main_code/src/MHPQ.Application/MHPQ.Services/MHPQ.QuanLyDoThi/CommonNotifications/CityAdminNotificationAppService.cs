using Abp;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.MultiTenancy;
using Abp.RealTime;
using MHPQ.Authorization.Users;
using MHPQ.Chat;
using MHPQ.Common.DataResult;
using MHPQ.Common.Enum;
using MHPQ.EntityDb;
using MHPQ.Notifications;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHPQ.Services
{
    public interface ICityAdminNotificationAppService : IApplicationService
    {
        Task<object> GetAllNotificationUserTenant(NotificationInput input);
        Task<object> GetAllFeedbackUser(GetFeedbackInput input);
        Task<object> UpdateStateFeedback(UserFeedbackDto input);
        Task<object> GetAllNotificationUser();
        Task<object> CreateOrUpdateNotification(CityNotificationDto input);
        Task<object> DeleteNotification(long id);
        Task<ListResultDto<UserFeedbackCommentDto>> GetAllCommnetByFeedback(long id);
        Task<object> CreateOrUpdateFeedbackComment(UserFeedbackCommentDto input);
        Task<object> DeleteFeedbackComment(long id);
    }

    public class CityAdminNotificationAppService : MHPQAppServiceBase, ICityAdminNotificationAppService
    {
        private readonly IRepository<CityNotification, long> _cityNotificationRepos;
        private readonly IRepository<CityNotificationComment, long> _commentRepos;
        private readonly IRepository<UserFeedback, long> _userFeedbackRepos;
        private readonly IOnlineClientManager _onlineClientManager;
        private readonly INotificationCommunicator _notificationCommunicator;
        private readonly IRepository<UserFeedbackComment, long> _userFeedbackCommentRepos;
        private readonly ITenantCache _tenantCache;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IRepository<Citizen, long> _citizenRepos;
        private readonly IRepository<User, long> _userRepos;
        private readonly UserManager _userManager;
        private readonly UserStore _store;

        public CityAdminNotificationAppService(
            IRepository<CityNotification, long> cityNotificationRepos,
            IRepository<Citizen, long> citizenRepos,
            IRepository<CityNotificationComment, long> commentRepos,
            IRepository<User, long> userRepos,
            IRepository<UserFeedback, long> userFeedbackRepos,
            IRepository<UserFeedbackComment, long> userFeedbackCommentRepos,
            IOnlineClientManager onlineClientManager,
            INotificationCommunicator notificationCommunicator,
            ITenantCache tenantCache,
            IUnitOfWorkManager unitOfWorkManager,
            UserManager userManager,
            UserStore store
            )
        {
            _cityNotificationRepos = cityNotificationRepos;
            _notificationCommunicator = notificationCommunicator;
            _onlineClientManager = onlineClientManager;
            _userFeedbackCommentRepos = userFeedbackCommentRepos;
            _commentRepos = commentRepos;
            _tenantCache = tenantCache;
            _unitOfWorkManager = unitOfWorkManager;
            _userFeedbackRepos = userFeedbackRepos;
            _userManager = userManager;
            _citizenRepos = citizenRepos;
            _store = store;
            _userRepos = userRepos;
        }

        public async Task<object> GetAllNotificationUserTenant(NotificationInput input)
        {
            try
            {

                var result = await _cityNotificationRepos.GetAllListAsync(x => x.Type == input.Type);
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

        public async Task<object> GetAllNotificationUser()
        {
            try
            {

                var result = await _cityNotificationRepos.GetAll()
                    .Where(x => x.UserId == AbpSession.UserId)
                    .OrderByDescending(m => m.CreationTime)
                    .Take(20)
                    .ToListAsync() ;
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

        [Obsolete]
        public async Task<object> CreateOrUpdateNotification(CityNotificationDto input)
        {
            try
            {
                long t1 = TimeUtils.GetNanoseconds();

                input.TenantId = AbpSession.TenantId;
                if (input.Id > 0)
                {
                    //update
                    var updateData = await _cityNotificationRepos.GetAsync(input.Id);
                    if (updateData != null)
                    {
                        input.MapTo(updateData);
                        //call back
                        await _cityNotificationRepos.UpdateAsync(updateData);
                    }
                    mb.statisticMetris(t1, 0, "Ud_noti");

                    var data = DataResult.ResultSucces(updateData, "Update success !");
                    return data;
                }
                else
                {

                    var insertInput = input.MapTo<CityNotification>();
                    long id = await _cityNotificationRepos.InsertAndGetIdAsync(insertInput);
                    insertInput.Id = id;
                    //var users = await _store.GetAllUserTenantAsync();
                    var clients = _onlineClientManager.GetAllClients()
                        .Where(c => c.TenantId == AbpSession.TenantId)
                        .ToImmutableList();
                    //var userOffines = (from us in users
                    //                   join ol in clients on us.Id equals ol.UserId into tb_ol
                    //                   from ol in tb_ol.DefaultIfEmpty()
                    //                   select new UserOffline()
                    //                   {
                    //                       Id = us.Id,
                    //                       IsOnline = ol != null ? true : false
                    //                   }).Where(x => x.IsOnline == false).ToList();

                    if (clients.Any())
                    {
                        _notificationCommunicator.SendNotificaionToUserTenant(clients, insertInput);
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

        public async Task<object> DeleteNotification(long id)
        {
            try
            {

                await _cityNotificationRepos.DeleteAsync(id);
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

        public async Task<object> GetAllFeedbackUser(GetFeedbackInput input)
        {
            try
            {
                var query = (from fb in _userFeedbackRepos.GetAll()
                             join us in _userRepos.GetAll() on fb.CreatorUserId equals us.Id into tb_us
                             from us in tb_us.DefaultIfEmpty()
                             select new UserFeedbackDto()
                             { 
                                 Id = fb.Id,
                                 CreationTime = fb.CreationTime,
                                 CreatorUserId = fb.CreatorUserId,
                                 Data = fb.Data,
                                 FileUrl = fb.FileUrl,
                                 Name = fb.Name,
                                 State = fb.State,
                                 LastModificationTime = fb.LastModificationTime,
                                 Type = fb.Type,
                                 FinishTime = fb.FinishTime,
                                 TenantId = fb.TenantId,
                                 UserName = us != null ? us.UserName: null,
                                 FullName = us != null ? us.FullName : null,
                                 ImageUrl = us != null ? us.ImageUrl : null,

                             })
                             .AsQueryable();

                #region Truy van tung Form

                switch (input.FormId)
                {
                    //cua hang moi dang ky
                    case (int)UserFeedbackEnum.FORM_ID_FEEDBACK.FORM_GETALL_NEW:
                        query = query.Where(x => x.State == null || x.State == (int)UserFeedbackEnum.STATE_FEEDBACK.NEW).OrderByDescending(x => x.CreationTime);
                        break;
                    case (int)UserFeedbackEnum.FORM_ID_FEEDBACK.FORM_GETALL_RECEIVE:
                        query = query.Where(x => x.State == (int)UserFeedbackEnum.STATE_FEEDBACK.RECEIVE).OrderByDescending(x => x.CreationTime);
                        break;
                    case (int)UserFeedbackEnum.FORM_ID_FEEDBACK.FORM_GETALL_COMPLETED:
                        query = query.Where(x => x.State == (int)UserFeedbackEnum.STATE_FEEDBACK.COMPLETED || x.State == (int)UserFeedbackEnum.STATE_FEEDBACK.RATING).OrderByDescending(x => x.CreationTime);
                        break;
                    case (int)UserFeedbackEnum.FORM_ID_FEEDBACK.FORM_GETALL_HANDLING:
                        query = query.Where(x => x.State == (int)UserFeedbackEnum.STATE_FEEDBACK.HANDLING).OrderByDescending(x => x.CreationTime);
                        break;
                 
                    default:
                        query = null;
                        break;
                }

                #endregion

                var result = await query.ToListAsync();
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

        //[Obsolete]
        //public async Task<object> CreateAswerFeedback(UserFeedbackDto input)
        //{
        //}

        [Obsolete]
        public async Task<ListResultDto<UserFeedbackCommentDto>> GetAllCommnetByFeedback(long id)
        {
            try
            {

                var result = await _userFeedbackCommentRepos.GetAll()
                    .Where(x => x.FeedbackId == id)
                    .OrderBy(m => m.CreationTime)
                    .Take(20)
                    .ToListAsync();

                  return new ListResultDto<UserFeedbackCommentDto>(result.MapTo<List<UserFeedbackCommentDto>>());
            }
            catch (Exception e)
            {
               // var data = DataResult.ResultError(e.ToString(), "Exception !");
                Logger.Fatal(e.Message);
                return null;
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
                    var citizen = new UserIdentifier(AbpSession.TenantId, input.CreatorFeedbackId);

                    var clients = _onlineClientManager.GetAllByUserId(citizen);

                    if (clients.Any())
                    {
                        _notificationCommunicator.SendCommentFeedbackToUserTenant(clients, insertInput);
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

        [Obsolete]
        public async Task<object> UpdateStateFeedback(UserFeedbackDto input)
        {
            try
            {
                long t1 = TimeUtils.GetNanoseconds();

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
            catch (Exception e)
            {
                var data = DataResult.ResultError(e.ToString(), "Exception !");
                Logger.Fatal(e.Message);
                return data;
            }
        }
    }
}
