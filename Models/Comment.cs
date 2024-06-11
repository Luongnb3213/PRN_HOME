using System.ComponentModel.DataAnnotations;

namespace PRN221_Assignment.Models
{
    public class Comment
    {
        [Key]
        public int CommentId { get; set; }
        public string Content { get; set; }

        public int React { get; set; }
        public int AuthorId { get; set; }

        public DateTime CreatedAt { get; set; }
        public virtual Account Account { get; set; }
        public virtual ICollection<CommentImages> CommentImages { get; set; }
        public virtual ICollection<Thread> Threads { get; set; }
        public virtual ICollection<ThreadComment> ThreadComments { get; set; }
    }
}
