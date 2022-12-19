using Api.Models.Attach;
using Api.Models.Comment;
using Api.Models.Like;
using Api.Models.User;

namespace Api.Models.Post
{
    public class PostModel
    {
        public Guid Id { get; set; }
        public string? Description { get; set; }
        public UserAvatarModel Author { get; set; } = null!;
        public List<LikeModel>? Likes { get; set; } = new List<LikeModel>();
        public List<CommentModel>? Comments { get; set; } = new List<CommentModel>();
        public List<AttachExternalModel>? Contents { get; set; } = new List<AttachExternalModel>();

        public int CommentsCount { get; set; }
        public int LikesCount { get; set; }
    }
}
