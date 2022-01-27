using Abp.AutoMapper;
using MHPQ.EntityDb;

namespace MHPQ.Services
{
    [AutoMap(typeof(UserFeedback))]
    public class UserFeedbackDto : UserFeedback
    {
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string ImageUrl { get; set; }
        public long UserId { get; set; }

    }

    [AutoMap(typeof(UserFeedbackComment))]
    public class UserFeedbackCommentDto: UserFeedbackComment
    {
        public string FullName { get; set; }
        public string ImageUrl { get; set; }
        public long CreatorFeedbackId { get; set; }
    }

    public class GetFeedbackInput
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public int FormId { get; set; }
        public int? State { get; set; }
        public long UserId { get; set; }
    }
}
