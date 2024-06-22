using System.ComponentModel.DataAnnotations;
using static System.Reflection.Metadata.BlobBuilder;
using System.Threading;
using System.Xml.Linq;

namespace PRN221_Assignment.Models
{
    public class Group
    {
        public Group()
        {
            GroupUser = new HashSet<GroupUser>();
            MessageReceive = new HashSet<MessageReceive>();
        }
        [Key]
        public int GroupId { get; set; }

        public string Name { get; set; }

        public string  Image { get; set; }

        public DateTime createdBy { get; set; }

        public virtual ICollection<GroupUser> GroupUser { get; set; }
        public virtual ICollection<Account> Accounts { get; set; }

        public virtual ICollection<MessageReceive> MessageReceive { get; set; }


    }
}
