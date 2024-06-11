using System.ComponentModel.DataAnnotations;

namespace PRN221_Assignment.Models
{
    public class Thread
    {
        [Key]
  
        public int ThreadId { get; set; }
 
        public string Content { get; set; }
     
        public int AuthorId { get; set; }

        public int React { get; set; }
        public int Share { get; set; }

        public DateTime SubmitDate { get; set; }

        public virtual Account Account { get; set; }

        public virtual ICollection<ThreadImages> ThreadImages { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<ThreadComment> ThreadComments { get; set; } 
    }
}
