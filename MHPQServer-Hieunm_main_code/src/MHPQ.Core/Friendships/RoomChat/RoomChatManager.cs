using Abp;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using MHPQ.Chat;
using MHPQ.Friendships;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHPQ.RoomChats
{
    public class RoomChatManager: MHPQDomainServiceBase, IRoomChatManager
    {

        //private readonly IRepository<Friendship, long> _friendshipRepository;
        //private readonly IRepository<ChatMessage, long> _chatMessageRepository;
        private readonly IRepository<RoomChat, long> _roomChatRepository;
       // private readonly IRepository<RoomMessage, long> _roomMessageRepository;
        private readonly IRepository<RoomUserChat, long> _roomUserChatRepository;

        public RoomChatManager(

            IRepository<RoomChat, long> roomChatRepository,

            IRepository<RoomUserChat, long> roomUserChatRepository)
        {
            _roomChatRepository = roomChatRepository;
            //_roomMessageRepository = roomMessageRepository;
            //_friendshipRepository = friendshipRepository;
            //_chatMessageRepository = chatMessageRepository;
            _roomUserChatRepository = roomUserChatRepository;
        }

        [UnitOfWork]
        public void AddMembershipGroupChat(UserIdentifier member, long groupId)
        {
            using (CurrentUnitOfWork.SetTenantId(member.TenantId))
            {
                var groupChatUser = new RoomUserChat()
                {
                    UserId = member.UserId,
                    RoomChatId = groupId,
                    TenantId = member.TenantId
                };
                _roomUserChatRepository.Insert(groupChatUser);
                CurrentUnitOfWork.SaveChanges();
            }
           
        }

        public long CreatGroupChat(RoomChat room)
        {
            using(CurrentUnitOfWork.SetTenantId(room.TenantId))
            {
                var id = _roomChatRepository.InsertAndGetId(room);
                if( id > 0)
                {
                    room.Id = id;
                    room.RoomChatCode = GenerateCodeRoomChat(id);
                }
                return id;
            }
        }

        [UnitOfWork]
        public RoomUserChat GetMemberGroupChatOrNull(UserIdentifier member, long groupId)
        {
            using (CurrentUnitOfWork.SetTenantId(member.TenantId))
            {
                return _roomUserChatRepository.FirstOrDefault(user =>
                                    user.UserId == member.UserId &&
                                    user.TenantId == member.TenantId);
            }
        }

        public RoomChat GetRoomChat(long roomId)
        {
            var room =  _roomChatRepository.Get(roomId);
            return room;
        }

        public string GenerateCodeRoomChat(long id)
        {
            Random random = new Random();
            const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            string a = new string(Enumerable.Repeat(chars, 4)
              .Select(s => s[random.Next(s.Length)]).ToArray());
            string b = id.ToString().PadLeft(3, '0');
            var checkCode = (a + b).ToString().Trim();

            var room = _roomChatRepository.FirstOrDefault(r => r.RoomChatCode == checkCode);
            if(room != null)
            {
                return GenerateCodeRoomChat(id); 
            }
            else
            {
                return checkCode;
            }

        }

        public void SendGroupChatMessage(UserIdentifier sender, string roomChatId, string message, string senderTenancyName, string senderUserName, Guid? senderProfilePictureId)
        {

            //HandleSenderToReceiver(sender, receiver, message);
            //HandleReceiverToSender(sender, receiver, message);
            //HandleSenderUserInfoChange(sender, receiver, senderTenancyName, senderUserName, senderProfilePictureId);
        }
    }
}
