using System.ComponentModel.DataAnnotations;
using System.Threading;

namespace PRN221_Assignment.Models
{
    public class Comment
    {
        public Comment() {
            CommentImages =  new HashSet<CommentImages>();
            Threads = new HashSet<Thread>();
            ThreadComments = new HashSet<ThreadComment>();
        }
        [Key]
        public int CommentId { get; set; }
        public string Content { get; set; }

        public int React { get; set; }
        public int AuthorId { get; set; }

        public DateTime CreatedAt { get; set; }
        public virtual Account? Account { get; set; }
        public virtual ICollection<CommentImages> CommentImages { get; set; }
        public virtual ICollection<Thread> Threads { get; set; }
        public virtual ICollection<ThreadComment> ThreadComments { get; set; }
    }
}
