using Microsoft.Extensions.Hosting;
using System.ComponentModel.DataAnnotations;

namespace PRN221_Assignment.Models
{
    public class ThreadComment
    {
        [Key]
        public int ThreadCommentId { get; set; }
        public int CommentId { get; set; }
        public int ThreadId { get; set; }
        public virtual Thread Thread { get; set; }
        public virtual Comment Comment { get; set; }
        public virtual ICollection<Conversation> Conversations { get; set; }

    }
}
