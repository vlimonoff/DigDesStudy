namespace Api.Models.Like
{
    public class CreateLikeModel
    {
        public Guid Id { get; set; }
        public Guid AuthorId { get; set; }
        public Guid TargetId { get; set; }
    }

    public class CreateLikeRequest
    {
        public Guid? AuthorId { get; set; }
        public Guid TargetId { get; set; }
    }
}