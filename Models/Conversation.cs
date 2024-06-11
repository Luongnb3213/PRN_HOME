﻿using System.ComponentModel.DataAnnotations;

namespace PRN221_Assignment.Models
{
    public class Conversation
    {
        [Key]
        public int ThreadCommentId { get; set; }
        public int BoxCommentId { get; set; }

        public int CommentId { get; set; }
        public virtual ThreadComment ThreadComment { get; set; }

    }
}
