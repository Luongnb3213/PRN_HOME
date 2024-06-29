using System.ComponentModel.DataAnnotations;

namespace PRN221_Assignment.Models
{
    public class MessageReceive
    {
        [Key]
        public int MessageReceiveId { get; set; }
        public int messID { get; set; }
        public int GroupID { get; set; }
        public int UserId { get; set; }
        public bool seen { get; set; }
        public bool type { get; set; }

        public virtual Account? Author { get; set; }
        public virtual Group? Group { get; set; }
        public virtual Mess? Mess { get; set; }


    }
}
