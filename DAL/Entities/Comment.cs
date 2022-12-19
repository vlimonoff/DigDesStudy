namespace DAL.Entities
{
    public class Comment
    {
        public Guid Id { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public string Content { get; set; } = null!;

        public Guid AuthorId { get; set; }
        public Guid PostId { get; set; }
        public virtual User Author { get; set; } = null!;
        public virtual Post Post { get; set; } = null!;
        public virtual ICollection<CommentReply>? CommentReplies { get; set; }
        public virtual ICollection<Like>? Likes { get; set; }
    }    
}
