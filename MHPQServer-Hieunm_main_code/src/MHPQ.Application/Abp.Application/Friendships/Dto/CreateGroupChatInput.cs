using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHPQ.Friendships.Dto
{
    public class CreateGroupChatInput
    {

        public string GroupName { get; set; }

        public List<FriendDto> MemberShips { get; set; }

        public Guid? PictureProfileId { get; set; }

    }
}
