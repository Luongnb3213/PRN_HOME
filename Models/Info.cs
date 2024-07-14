using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PRN221_Assignment.Models
{
    public class Info
    {
        [Key]
        public int UserID { get; set; }

        [Required]
        public string userName { get; set; }

        [RegularExpression(@"^[a-zA-Z]{2,}$", ErrorMessage = "Name must contain only letters and be at least 2 characters long.")]
        public string Name { get; set; }
        public string? Story { get; set; }

        public DateTime Dob { get; set; }
        
        public string? Image { get; set; }
       
     
        public virtual Account? Account { get; set; }

    }
}
