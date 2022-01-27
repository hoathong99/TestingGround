using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using MHPQ.Chat.Dto;

namespace MHPQ.Chat
{
    public interface IChatAppService : IApplicationService
    {
        GetUserChatFriendsWithSettingsOutput GetUserChatFriendsWithSettings();
        GetUserChatFriendsWithSettingsOutput GetFriendRequestingList();

        Task<ListResultDto<ChatMessageDto>> GetUserChatMessages(GetUserChatMessagesInput input);

        Task MarkAllUnreadMessagesOfUserAsRead(MarkAllUnreadMessagesOfUserAsReadInput input);

    }
}
