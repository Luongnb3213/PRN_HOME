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

    }
}
