using Microsoft.Extensions.Hosting;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PRN221_Assignment.Models
{
    public class Account
    {
        [Key]
        public int UserID { get; set; }
        public string Email { get; set; }

        public string Password { get; set; }

        public bool Status { get; set; }

        public bool isActive { get; set; }

        public virtual Info Info { get; set; }
        public virtual ICollection<Follow> Follows { get; set; }
        public virtual ICollection<Block> BLocks { get; set; }

        public virtual ICollection<Thread> Threads { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
    }
}
