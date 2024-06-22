using System.ComponentModel.DataAnnotations;

namespace PRN221_Assignment.Models
{
    public class ThreadReact
    {

        [Key]
        public int ThreadReactId { get; set; }
        public int UserID { get; set; }
        public int threadId { get; set; }

        public virtual Thread? Thread { get; set; }
    }
}
