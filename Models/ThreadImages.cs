using System.ComponentModel.DataAnnotations;

namespace PRN221_Assignment.Models
{
    public class ThreadImages
    {
        [Key]
        public int ThreadId { get; set; }
        public string Media { get; set; }
        public virtual Thread Thread { get; set; }
    }
}
