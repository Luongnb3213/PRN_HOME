using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PRN221_Assignment.Models;

namespace PRN221_Assignment.Pages.FollowersManagement
{
    public class IndexModel : PageModel
    {
        public readonly PRN221_Assignment.Respository.DataContext context;
        public IndexModel(PRN221_Assignment.Respository.DataContext _context)
        {
            context = _context;
        }
        [BindProperty(SupportsGet = true)]
        public int userId { get;set; }
        public List<Follow> myFollower { get; set; }
        public void OnGet()
        {
            OnGetGetListFollow();
        }
        public void OnGetGetListFollow()
        {
            myFollower = context.Follow.Include(x => x.Account).Include(x => x.Account.Info).Where(x => x.UserID == userId).ToList();
            //query lai sau
        }
    }
}
