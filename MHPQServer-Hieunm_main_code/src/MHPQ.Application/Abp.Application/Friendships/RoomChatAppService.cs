using Abp;
using Abp.Application.Services;
using Abp.Authorization;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.MultiTenancy;
using Abp.RealTime;
using Abp.Runtime.Session;
using Abp.Timing;
using MHPQ.Chat;
using MHPQ.Chat.Dto;
using MHPQ.Friendships;
using MHPQ.Friendships.Dto;
using MHPQ.RoomChats;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace MHPQ.Abp.Application.Friendships
{
    

    [AbpAuthorize]
    public class RoomChatAppService : MHPQAppServiceBase, IRoomChatAppService
    {

        private readonly IOnlineClientManager _onlineClientManager;
        private readonly IChatCommunicator _chatCommunicator;
        private readonly ITenantCache _tenantCache;
        private readonly IChatFeatureChecker _chatFeatureChecker;
        private readonly IRoomChatManager _roomChatManager;
        private readonly IRepository<Friendship, long> _friendshipRepository;
        private readonly IRepository<ChatMessage, long> _chatMessageRepository;
        private readonly IRepository<RoomChat, long> _roomChatRepository;
        private readonly IRepository<RoomMessage, long> _roomMessageRepository;
        private readonly IRepository<RoomUserChat, long> _roomUserChatRepository;

        public RoomChatAppService(
            IOnlineClientManager onlineClientManager,
            IChatCommunicator chatCommunicator,
            ITenantCache tenantCache,
            IChatFeatureChecker chatFeatureChecker,
            IRoomChatManager roomChatManager,
            IRepository<Friendship, long> friendshipRepository,
            IRepository<ChatMessage, long> chatMessageRepository,
            IRepository<RoomChat, long> roomChatRepository,
            IRepository<RoomMessage, long> roomMessageRepository,
            IRepository<RoomUserChat, long> roomUserChatRepository)
        {
            _onlineClientManager = onlineClientManager;
            _chatCommunicator = chatCommunicator;
            _tenantCache = tenantCache;
            _chatFeatureChecker = chatFeatureChecker;
            _roomChatManager = roomChatManager;
            _roomChatRepository = roomChatRepository;
            _roomMessageRepository = roomMessageRepository;
            _friendshipRepository = friendshipRepository;
            _chatMessageRepository = chatMessageRepository;
            _roomUserChatRepository = roomUserChatRepository;
        }

        public async Task<RoomChatUserDto> AddUserToGroupChat(AddUserGroupChatInput input)
        {
            try
            {
               
                var userIdentifier = AbpSession.ToUserIdentifier();
                var user = await UserManager.FindByIdAsync(AbpSession.GetUserId().ToString());
                var groupchat = await _roomChatRepository.FirstOrDefaultAsync(x =>( x.Id == input.GroupId && x.AdminId == AbpSession.GetUserId()));
                if (input.MemberShips != null && groupchat != null)
                {
                    foreach( var member in input.MemberShips)
                    {
                        var friend = new UserIdentifier(member.TenantId, member.FriendId);
                        if(_roomChatManager.GetMemberGroupChatOrNull(friend, input.GroupId) == null)
                        {
                            _roomChatManager.AddMembershipGroupChat(friend, input.GroupId);
                        }

                        var clients = _onlineClientManager.GetAllByUserId(friend);
                        if (clients.Any())
                        {
                            var isFriendOnline = _onlineClientManager.IsOnline(friend);
                            _chatCommunicator.SendNotificationAddGroupToUserClient(groupchat.RoomChatCode, friend);
                        }

                        //var senderClients = _onlineClientManager.GetAllByUserId(userIdentifier);
                        //if (senderClients.Any())
                        //{
                        //    var isFriendOnline = _onlineClientManager.IsOnline(userIdentifier);
                        //    _chatCommunicator.SendNotificationAddGroupToUserClient(senderClients, userIdentifier, isFriendOnline);
                        //}
                    };
                }

                return null;
            }
            catch(Exception ex)
            { 
                Logger.Fatal(ex.Message);
                return null;
            }
        }

        public async Task<object> CreateGroupChat(CreateGroupChatInput input)
        {
            try
            {
                var user = AbpSession.ToUserIdentifier();
                //var groupchat = await _roomChatRepository.FirstOrDefaultAsync(r => r.Name == input.GroupName);
                //if(groupchat != null)
                //{
                //    return "GroupNameIsExisting";
                //}
                var roomChat = new RoomChat(user,input.GroupName, input.PictureProfileId);
                var groupId = _roomChatManager.CreatGroupChat(roomChat);
                var initmember = new UserIdentifier(user.TenantId, user.UserId);
                _roomChatManager.AddMembershipGroupChat(initmember, groupId);
                var userClients = _onlineClientManager.GetAllByUserId(user);
                if (userClients.Any())
                {
                    var isFriendOnline = _onlineClientManager.IsOnline(user);
                    _chatCommunicator.AddUserToGroupChat(userClients, roomChat.RoomChatCode, user);
                }

                if (input.MemberShips != null)
                {
                    foreach(var member in input.MemberShips)
                    {
                        var friend = new UserIdentifier(member.FriendTenantId, member.FriendUserId);
                        if (_roomChatManager.GetMemberGroupChatOrNull(friend, groupId) == null)
                        {
                            _roomChatManager.AddMembershipGroupChat(friend, groupId);
                        }

                        var memberClients = _onlineClientManager.GetAllByUserId(friend);
                        if (memberClients.Any())
                        {
                            var isFriendOnline = _onlineClientManager.IsOnline(friend);
                            _chatCommunicator.AddUserToGroupChat(memberClients, roomChat.RoomChatCode, friend);
                        }

                    };
                    _chatCommunicator.SendNotificationCreateGroupToUserClient(roomChat.RoomChatCode, user);
                    //var 
                }
                    return null;
            }
            catch (Exception ex)
            {
                Logger.Fatal(ex.Message);
                return null;
            }
        }

        public async Task<List<RoomChatDto>> GetAllGroupChats()
        {
            try
            {

                var userId = AbpSession.GetUserId();

                var allGroups = (from gp in _roomChatRepository.GetAll()
                                 where gp.AdminId == userId
                                 select new RoomChatDto()
                                 {
                                     AdminId = gp.AdminId,
                                     AdminName= gp.AdminName,
                                     CreationTime = gp.CreationTime,
                                     Id = gp.Id,
                                     Name = gp.Name,
                                     ProfilePictureId = gp.ProfilePictureId,
                                     TenantId = gp.TenantId
                                     
                                 }).ToList();

                foreach (var group in allGroups)
                {
                    var members = _roomUserChatRepository.GetAllList(m => (m.RoomChatId == group.Id && m.UserId != userId));
                    foreach(var member in members)
                    {

                        if(_onlineClientManager.IsOnline(new UserIdentifier(member.TenantId, member.UserId)))
                        {
                            group.IsOnline = true;
                            break;
                        }
                    }

                    group.UnreadMessageCount = _roomMessageRepository.GetAll()
                     .Where(m => (m.UserId == userId && m.GroupId
                     == group.Id && m.ReadState == ChatMessageReadState.Unread))
                     .OrderByDescending(m => m.CreationTime)
                     .Take(10)
                     .ToList()
                     .Count();
                }

                return allGroups;
            }
            catch (Exception ex)
            {
                Logger.Fatal(ex.Message);
                return null;
            }
        }

        [Obsolete]
        public async Task<object> GetMessageGroupChat(GetMessageGroupChatInput input)
        {
            try
            {
                var userId = AbpSession.GetUserId();
                var messages = await _roomMessageRepository.GetAll()
                        .Where(m => (m.UserId == userId && m.GroupId == input.GroupChatId))
                        .OrderByDescending(m => m.CreationTime)
                        .Take(20)
                        .ToListAsync();

                messages.Reverse();

                return new List<RoomMessageDto>(messages.MapTo<List<RoomMessageDto>>());
            }
            catch (Exception ex)
            {
                Logger.Fatal(ex.Message);
                return null;
            }
        }

       
    }
}
