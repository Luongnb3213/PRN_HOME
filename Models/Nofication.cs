using System.ComponentModel.DataAnnotations;

namespace PRN221_Assignment.Models
{
    public class Nofication
    {
        public Nofication() {
            UserNofications = new HashSet<UserNofication>();
            AccountsNofication = new HashSet<Account>();
        }
        [Key]
        public int Id { get; set; }
        public int typeID { get; set; }

        public int authorId { get; set; }
        public DateTime createdBy { get; set; }
        public int dataId { get; set; }


        public virtual Account? Account { get; set; }
        public virtual typeNofication? typeNofication { get; set; }
        public virtual ICollection<Account> AccountsNofication { get; set; }

        public virtual ICollection<UserNofication> UserNofications { get; set; }
    }
}
