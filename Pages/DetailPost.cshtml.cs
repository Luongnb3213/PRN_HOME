﻿using Microsoft.AspNetCore.Mvc;
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
        public List<dynamic> listReplyDetail { get; set; }
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

            int totalOriginal = context.ThreadComment.Count(x => x.ThreadId == ThreadId);
            int totalReply = context.Conversation
                        .Count(x => context.ThreadComment
                        .Select(tc => tc.ThreadCommentId)
                        .Contains(x.ThreadCommentId));

            numOfComment = totalOriginal + totalReply;

            listOriginalComment = context.ThreadComment.Where(x => x.ThreadId == ThreadId)
                .Include(x => x.Comment)
                .ThenInclude(x => x.CommentImages)
                .Include(x => x.Conversations)
                .OrderByDescending(x => x.Comment.CreatedAt)
                .ToList();

            //List<dynamic> listReplyall = (from a in context.Comment
            //                              join b in context.Conversation on a.CommentId equals b.CommentId
            //                              join c in context.ThreadComment on b.ThreadCommentId equals c.ThreadCommentId
            //                              join d in context.Accounts on a.AuthorId equals d.UserID
            //                              join e in context.Info on d.UserID equals e.UserID
            //                              join f in context.CommentImages on a.CommentId equals f.CommentId
            //                              select new
            //                              {
            //                                  ReplyId = a.CommentId,
            //                                  a.Content,
            //                                  a.React,
            //                                  a.AuthorId,
            //                                  a.CreatedAt,
            //                                  b.ConversationId,
            //                                  b.ThreadCommentId,
            //                                  OriginalId = c.CommentId,
            //                                  DetailThreadId = c.ThreadId,
            //                                  d.UserID,
            //                                  e.userName,
            //                                  e.Image,
            //                                  f.Media
            //                              }).ToList().Cast<dynamic>().ToList();
            var fullJoin = (from a in context.Comment
                            join b in context.Conversation on a.CommentId equals b.CommentId into gjb
                            from subb in gjb.DefaultIfEmpty()
                            join c in context.ThreadComment on subb.ThreadCommentId equals c.ThreadCommentId into gjc
                            from subc in gjc.DefaultIfEmpty()
                            join d in context.Accounts on a.AuthorId equals d.UserID into gjd
                            from subd in gjd.DefaultIfEmpty()
                            join e in context.Info on subd.UserID equals e.UserID into gje
                            from sube in gje.DefaultIfEmpty()
                            join f in context.CommentImages on a.CommentId equals f.CommentId into gjf
                            from subf in gjf.DefaultIfEmpty()
                            select new
                            {
                                ReplyId = a.CommentId,
                                Content = a.Content,
                                React = a.React,
                                AuthorId = a.AuthorId,
                                CreatedAt = a.CreatedAt,
                                ConversationId = subb != null ? subb.ConversationId : (int?)null,
                                ThreadCommentId = subb != null ? subb.ThreadCommentId : (int?)null,
                                OriginalId = subc != null ? subc.CommentId : (int?)null,
                                DetailThreadId = subc != null ? subc.ThreadId : (int?)null,
                                UserID = subd != null ? subd.UserID : (int?)null,
                                userName = sube != null ? sube.userName : null,
                                Image = sube != null ? sube.Image : null,
                                Media = subf != null ? subf.Media : null
                            }).ToList();

            List<dynamic> listReplyall = fullJoin.Cast<dynamic>().ToList();

            listReplyDetail = listReplyall.Where(x => x.DetailThreadId == ThreadId).ToList();
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
            }
            else
            {
                Conversation newConversation = new Conversation();
                newConversation.CommentId = newComment.CommentId;
                newConversation.ThreadCommentId = Int32.Parse(comment.ThreadCommentId);
                context.Conversation.Add(newConversation);
                context.SaveChanges();
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

            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve
            };

            if (comment.Type.Equals("original"))
            {
                ThreadComment newOriginalComment = context.ThreadComment
                    .Include(x => x.Comment)
                    .Include(x => x.Conversations)
                    .Include(x => x.Comment.Account.Info)
                    .Include(x => x.Comment.CommentImages)
                    .FirstOrDefault(x => x.CommentId == newComment.CommentId);

                return new JsonResult(new { data = newOriginalComment }, options);
            }
            else
            {
                int maxConversationId = context.Conversation.Max(x => x.ConversationId);
                var currentReply = context.Conversation.FirstOrDefault(x => x.ConversationId == maxConversationId);
                Comment newReplyComment = context.Comment
                    .Include(x => x.ThreadComments)
                    .Include(x => x.CommentImages)
                    .Include(x => x.Account)
                    .Include(x => x.Account.Info)
                    .FirstOrDefault(x => x.CommentId == currentReply.CommentId);
                return new JsonResult(new { data = newReplyComment }, options);
            }

        }
        public class MyComment()
        {
            public string Content { get; set; }
            public IFormFile Pictures { get; set; }
            public string Type { get; set; }
            public string? ThreadCommentId { get; set; }
        }
    }
}
