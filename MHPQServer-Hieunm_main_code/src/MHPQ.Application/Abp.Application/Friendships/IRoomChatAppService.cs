using Abp.Application.Services;
using MHPQ.Friendships.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MHPQ.Abp.Application.Friendships
{
    public interface IRoomChatAppService : IApplicationService
    {
        Task<RoomChatUserDto> AddUserToGroupChat(AddUserGroupChatInput input);
        Task<object> CreateGroupChat(CreateGroupChatInput input);
        Task<List<RoomChatDto>> GetAllGroupChats();
        Task<object> GetMessageGroupChat(GetMessageGroupChatInput input);
    }
}
