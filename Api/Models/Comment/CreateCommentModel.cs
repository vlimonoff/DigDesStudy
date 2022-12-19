using Api.Models.User;

namespace Api.Models.Comment
{
    public class CreateCommentModel
    {
        public Guid Id { get; set; }
        public Guid AuthorId { get; set; }
        public Guid PostId { get; set; }
        public string Content { get; set; } = null!;
    }

    public class CreateCommentRequest
    {
        public Guid? AuthorId { get; set; }
        public Guid PostId { get; set; }
        public string Content { get; set; } = null!;
    }
}
