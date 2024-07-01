using System.ComponentModel.DataAnnotations;
using static System.Reflection.Metadata.BlobBuilder;
using System.Threading;
using System.Xml.Linq;

namespace PRN221_Assignment.Models
{
    public class typeNofication
    {


        public typeNofication()
        {
            Nofications = new HashSet<Nofication>();
        }
        [Key]
        public int Id { get; set; }

        public string? content { get; set; }

        public virtual ICollection<Nofication> Nofications { get; set; }
    }
}
