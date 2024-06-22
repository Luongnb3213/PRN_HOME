using System.ComponentModel.DataAnnotations;
using static System.Reflection.Metadata.BlobBuilder;
using System.Threading;
using System.Xml.Linq;

namespace PRN221_Assignment.Models
{
    public class Mess
    {


        [Key]
        public int messId { get; set; }
        public string Content { get; set; }

        public int AuthorId { get; set; }

        public DateTime createdBy { get; set; }

        public bool type { get; set; }

        public virtual Account? Author { get; set; }
        public virtual MessageReceive? MessageReceive { get; set; }

    }
}
