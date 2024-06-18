using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PRN221_Assignment.Models;
using System.Text.Json.Serialization;
using System.Text.Json;

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
                .OrderByDescending(x => x.Comment.CreatedAt)
                .ToList();

        }
        public async Task<IActionResult> OnPost([FromForm] MyComment comment)
        {
            Comment newComment = new Comment();
            newComment.Content = comment.Content == null ? string.Empty : comment.Content;
            newComment.React = 0;
            newComment.AuthorId = 1; //Cái này sau phải chuyển theo account khác
            newComment.CreatedAt = DateTime.Now;
            context.Comment.Add(newComment);
            context.SaveChanges();

            if (comment.Type.Equals("original"))
            {
            ThreadComment newThreadComment = new ThreadComment();
            newThreadComment.CommentId = newComment.CommentId;
            newThreadComment.ThreadId = ThreadId;
            context.ThreadComment.Add(newThreadComment);
            context.SaveChanges();
            } else
            {

            }

            if (comment.Pictures != null)
            {
                var filePath = Path.Combine("wwwroot/commentMedia", comment.Pictures.FileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await comment.Pictures.CopyToAsync(stream);
                }
                CommentImages newCommentImage = new CommentImages();
                newCommentImage.CommentId = newComment.CommentId;
                newCommentImage.Media = "commentMedia/" + comment.Pictures.FileName;
                context.CommentImages.Add(newCommentImage);
                context.SaveChanges();
            }

            ThreadComment newOriginalComment = context.ThreadComment
                .Include(x => x.Comment)
                .Include(x => x.Conversations)
                .Include(x => x.Comment.Account.Info)
                .Include(x => x.Comment.CommentImages)
                .FirstOrDefault(x => x.CommentId == newComment.CommentId);

            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve
            };

            return new JsonResult(new { data = newOriginalComment }, options);
        }
        public class MyComment()
        {
            public string Content { get; set; }
            public IFormFile Pictures { get; set; }
            public string Type { get; set; }
        }
    }
}
