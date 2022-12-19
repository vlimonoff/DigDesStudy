using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class CommentReply
    {
        public Guid Id { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public string Content { get; set; } = null!;
        public Guid AuthorId { get; set; }
        public Guid? ParentCommentId { get; set; }
        public virtual User Author { get; set; } = null!;
        public virtual Comment? ParentComment { get; set; }
        public virtual ICollection<Like>? Likes { get; set; }
    }
}
