using Api.Models.Like;
using Api.Models.User;

namespace Api.Models.Comment
{
    public class CommentReplyModel
    {
        public Guid Id { get; set; }
        public UserAvatarModel Author { get; set; } = null!;
        public string Content { get; set; } = null!;
        public List<LikeModel>? Likes { get; set; } = new List<LikeModel>();
        public int LikesCount { get; set; }
    }
}
