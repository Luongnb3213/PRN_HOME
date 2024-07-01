using System.ComponentModel.DataAnnotations;

namespace PRN221_Assignment.Models
{
    public class UserNofication
    {
        [Key]
        public int UserNoficationId { get; set; }
        public int UserId { get; set; }

        public int NoficationId { get; set; }
        public virtual Account? User { get; set; }
        public virtual Nofication? Nofication { get; set; }
    }
}
