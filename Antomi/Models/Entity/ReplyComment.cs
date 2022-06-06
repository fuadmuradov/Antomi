using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Antomi.Models.Entity
{
    public class ReplyComment
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public int BlogCommentId { get; set; }
        public BlogComment BlogComment { get; set; }
        [NotMapped]
        public string AppUserId1 { get; set; }
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
