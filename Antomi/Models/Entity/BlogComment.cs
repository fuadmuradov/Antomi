using FluentValidation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Antomi.Models.Entity
{
    public class BlogComment
    {
        public int Id { get; set; }
        [NotMapped]
        public string AppUserId1 { get; set; }
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }
        public string Text { get; set; }
        public int BlogId { get; set; }
        public Blog Blog { get; set; }
        public List<ReplyComment> ReplyComments { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class BlogCommentValidation : AbstractValidator<BlogComment>
    {
        public BlogCommentValidation()
        {
            RuleFor(x => x.BlogId).NotEmpty().NotNull();
            RuleFor(x => x.Text).NotEmpty().NotNull().MaximumLength(250);
        }
    }

}
