using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PRN221_Assignment.Models;

namespace PRN221_Assignment.Pages.Profile
{
    public class DetailPostModel : PageModel
    {
        public readonly PRN221_Assignment.Respository.DataContext context;
        public DetailPostModel(PRN221_Assignment.Respository.DataContext _context)
        {
            context = _context;
        }
        [BindProperty(SupportsGet = true)]
        public int ThreadId { get; set; }
        //[BindProperty]
        //public string Comment { get; set; }
        public Models.Thread SelectedThread { get; set; }
        public int numOfComment { get; set; }
        public List<ThreadComment> listOriginalComment { get; set; }
        public void OnGet()
        {
            SelectedThread = context.Thread
                .Include(x => x.Account)
                .ThenInclude(x => x.Info)
                .Include(x => x.ThreadImages)
                .Include(x => x.ThreadComments)
                .Include(x => x.Comments)
                .Include(x => x.Account.Info)
                .FirstOrDefault(x => x.ThreadId == ThreadId);

            numOfComment = context.ThreadComment.Count(x => x.ThreadId == ThreadId);

            listOriginalComment = context.ThreadComment.Where(x => x.ThreadId == ThreadId)
                .Include(x => x.Comment)
                .ThenInclude(x => x.CommentImages)
                .Include(x => x.Conversations)
                .ToList();

        }
        public IActionResult OnPost([FromBody] string comment)
        {
            Comment newComment = new Comment();
            newComment.Content = comment;
            newComment.React = 0;
            newComment.AuthorId = 1;
            newComment.CreatedAt = DateTime.Now;
            context.Comment.Add(newComment);
            context.SaveChanges();

            ThreadComment newThreadComment = new ThreadComment();
            newThreadComment.CommentId = newComment.CommentId;
            newThreadComment.ThreadId = ThreadId;
            context.ThreadComment.Add(newThreadComment);
            context.SaveChanges();

            ThreadComment newOriginalComment = context.ThreadComment
                .Include(x => x.Comment)
                .Include(x => x.Conversations)
                .Include(x => x.Comment.Account.Info)
                .FirstOrDefault(x => x.CommentId == newComment.CommentId);

            return new JsonResult(newOriginalComment);
        }
    }
}
