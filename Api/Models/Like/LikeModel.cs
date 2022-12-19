using Api.Models.User;

namespace Api.Models.Like
{
    public class LikeModel
    {
        public Guid Id { get; set; }
        public UserAvatarModel Author { get; set; } = null!;
        public Guid TargetId { get; set; }
    }
}
