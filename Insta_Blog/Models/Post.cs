using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Insta_Blog.Models
{
    public class Post
    {
        public int Id { get; set; }
        public DateTime ExecTime { get; set; }
        public string Description { get; set; }
        public  string AppUserId{ get; set; }
        public virtual AppUser AppUser { get; set; }
        public virtual ICollection<LikeCount> PostCounts { get; set; }
        public virtual ICollection<PostCommet> PostCommets { get; set; }


        public string ImageUrl { get; set; }
        [NotMapped]
        [Required]
        public IFormFile Photo { get; set; }

        [NotMapped]
        public IFormFile PhotoUpdate { get; set; }

    }
}
