using System.Collections.Generic;
using Abp.Domain.Repositories;
using System.Linq;
using System.Threading.Tasks;
using Abp;
using Abp.Application.Services.Dto;
using Abp.Auditing;
using Abp.AutoMapper;
using Abp.Linq.Extensions;
using Abp.RealTime;
using Abp.Runtime.Session;
using Abp.Timing;
using MHPQ.Friendships.Cache;
using MHPQ.Chat.Dto;
using Microsoft.EntityFrameworkCore;
using MHPQ.Friendships.Dto;
using MHPQ.Friendships;

namespace MHPQ.Chat
{
    public class ChatAppService : MHPQAppServiceBase, IChatAppService
    {
        private readonly IRepository<ChatMessage, long> _chatMessageRepository;
        private readonly IUserFriendsCache _userFriendsCache;
        private readonly IOnlineClientManager _onlineClientManager;
        private readonly IChatCommunicator _chatCommunicator;
        public ChatAppService(
            IRepository<ChatMessage, long> chatMessageRepository,
            IUserFriendsCache userFriendsCache,
            IOnlineClientManager onlineClientManager,
            IChatCommunicator chatCommunicator)
        {
            _chatMessageRepository = chatMessageRepository;
            _userFriendsCache = userFriendsCache;
            _onlineClientManager = onlineClientManager;
            _chatCommunicator = chatCommunicator;
        }

        [DisableAuditing]
        [System.Obsolete]
        public GetUserChatFriendsWithSettingsOutput GetUserChatFriendsWithSettings()
        {
            var userId = AbpSession.GetUserId();
            //var cacheItem = _userFriendsCache.GetCacheItem(AbpSession.ToUserIdentifier());
            var cacheItem = _userFriendsCache.GetUserFriendsCacheItemInternal(AbpSession.ToUserIdentifier(), FriendshipState.Accepted);

            var friends = cacheItem.Friends.MapTo<List<FriendDto>>();

            foreach (var friend in friends)
            {
                friend.IsOnline = _onlineClientManager.IsOnline(
                    new UserIdentifier(friend.FriendTenantId, friend.FriendUserId)
                );

                friend.UnreadMessageCount =  _chatMessageRepository.GetAll()
                 .Where(m => (m.UserId == userId && m.TargetUserId == friend.FriendUserId && m.ReadState == ChatMessageReadState.Unread))
                 .OrderByDescending(m => m.CreationTime)
                 .Take(20)
                 .ToList()
                 .Count();
            }

            return new GetUserChatFriendsWithSettingsOutput
            {
                Friends = friends,
                ServerTime = Clock.Now
            };
        }

        [DisableAuditing]
        [System.Obsolete]
        public GetUserChatFriendsWithSettingsOutput GetFriendRequestingList()
        {
            var userId = AbpSession.GetUserId();
            //var cacheItem = _userFriendsCache.GetCacheItem(AbpSession.ToUserIdentifier());
            var cacheItem = _userFriendsCache.GetUserFriendsCacheItemInternal(AbpSession.ToUserIdentifier(), FriendshipState.Requesting);

            var friends = cacheItem.Friends.MapTo<List<FriendDto>>();

            foreach (var friend in friends)
            {
                friend.IsOnline = _onlineClientManager.IsOnline(
                    new UserIdentifier(friend.FriendTenantId, friend.FriendUserId)
                );

                friend.UnreadMessageCount = _chatMessageRepository.GetAll()
                 .Where(m => (m.UserId == userId && m.TargetUserId == friend.FriendUserId && m.ReadState == ChatMessageReadState.Unread))
                 .OrderByDescending(m => m.CreationTime)
                 .Take(20)
                 .ToList()
                 .Count();
            }

            return new GetUserChatFriendsWithSettingsOutput
            {
                Friends = friends,
                ServerTime = Clock.Now
            };
        }

        [DisableAuditing]
        [System.Obsolete]
        public async Task<ListResultDto<ChatMessageDto>> GetUserChatMessages(GetUserChatMessagesInput input)
        {
            var userId = AbpSession.GetUserId();
            var messages = await _chatMessageRepository.GetAll()
                    .WhereIf(input.MinMessageId.HasValue, m => m.Id < input.MinMessageId.Value)
                    .Where(m => (m.UserId == userId && m.TargetUserId == input.UserId) || (m.UserId == input.UserId && m.TargetUserId == userId))
                    .OrderByDescending(m => m.CreationTime)
                    .Take(20)
                    .ToListAsync();

            messages.Reverse();

            return new ListResultDto<ChatMessageDto>(messages.MapTo<List<ChatMessageDto>>());
        }

        public async Task MarkAllUnreadMessagesOfUserAsRead(MarkAllUnreadMessagesOfUserAsReadInput input)
        {
            var userId = AbpSession.GetUserId();
            var messages = await _chatMessageRepository
                .GetAll()
                .Where(m =>
                    m.UserId == userId &&
                    m.TargetTenantId == input.TenantId &&
                    m.TargetUserId == input.UserId &&
                    m.ReadState == ChatMessageReadState.Unread)
                .ToListAsync();

            if (!messages.Any())
            {
                return;
            }

            foreach (var message in messages)
            {
                message.ChangeReadState(ChatMessageReadState.Read);
            }

            var userIdentifier = AbpSession.ToUserIdentifier();
            var friendIdentifier = input.ToUserIdentifier();

            _userFriendsCache.ResetUnreadMessageCount(userIdentifier, friendIdentifier);

            var onlineClients = _onlineClientManager.GetAllByUserId(userIdentifier);
            if (onlineClients.Any())
            {
                _chatCommunicator.SendAllUnreadMessagesOfUserReadToClients(onlineClients, friendIdentifier);
            }
        }
    }
}