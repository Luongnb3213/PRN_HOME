using System.ComponentModel.DataAnnotations;

namespace PRN221_Assignment.Models
{
    public class Block
    {

        [Key]
        public int BlockId { get; set; }
        public int UserID { get; set; }

        public virtual Account Account { get; set; }
    }
}
