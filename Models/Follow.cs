using System.ComponentModel.DataAnnotations;

namespace PRN221_Assignment.Models
{
    public class Follow
    {
        [Key]
        public int FollowerId { get; set; }
        public int UserID { get; set; }

        public virtual Account? Account { get; set; }
    }
}
