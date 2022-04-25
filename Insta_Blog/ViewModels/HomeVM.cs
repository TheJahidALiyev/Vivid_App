using Insta_Blog.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace Insta_Blog.ViewModels
{
    public class HomeVM
    {
        public virtual ICollection<LikeCount>   LikeCounts { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
        public virtual ICollection<AppUser> AppUsers { get; set; }
        IList<AppUser> Users { get; set; }
        public virtual ICollection<PostCommet> PostCommets { get; set; }
        public virtual AppUser AppUser { get; set; }
        public string Text { get; set; }
        public int? TextId { get; set; }
        public int? LikeCount { get; set; }

        public string Description { get; set; }
        public IFormFile Photo { get; set; }
        public IFormFile PhotoUpd { get; set; }
    }
}
