using System.ComponentModel.DataAnnotations;

namespace PRN221_Assignment.Models
{
    public class GroupUser
    {
        [Key]
        public int GroupUserId { get; set; }
        public int UserId { get; set; }

        public int GroupId { get; set; }
        public virtual Account? User { get; set; }
        public virtual Group? Group { get; set; }
    }
}
