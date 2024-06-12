using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PRN221_Assignment.Models
{
    public class Info
    {
        [Key]
        public int UserID { get; set; }
        public string userName { get; set; }
        public string Name { get; set; }
        public string Story { get; set; }
        public DateTime Dob { get; set; }
        
        public string Image { get; set; }
       
     
        public virtual Account? Account { get; set; }

    }
}
