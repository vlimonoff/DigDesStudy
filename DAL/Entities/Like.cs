namespace DAL.Entities
{
    public class Like
    {
        public Guid Id { get; set; }
        public Guid AuthorId { get; set; }
        public virtual User Author { get; set; } = null!;
        public DateTimeOffset LikeDate { get; set; }
        public Guid TargetId { get; set; }
    }
}
