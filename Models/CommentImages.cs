using System.ComponentModel.DataAnnotations;

namespace PRN221_Assignment.Models
{
    public class CommentImages
    {
        [Key]
        public int CommentId { get; set; }

        public string Media {  get; set; }

        public virtual Comment? Comment { get; set; }
    }
}
