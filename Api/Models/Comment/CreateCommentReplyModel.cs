namespace Api.Models.Comment
{
    public class CreateCommentReplyModel
    {
        public Guid Id { get; set; }
        public Guid AuthorId { get; set; }
        public string Content { get; set; } = null!;
        public Guid ParentCommentId { get; set; }
    }

    public class CreateCommentReplyRequest
    {
        public Guid? AuthorId { get; set; }
        public Guid ParentCommentId { get; set; }
        public string Content { get; set; } = null!;
    }
}
