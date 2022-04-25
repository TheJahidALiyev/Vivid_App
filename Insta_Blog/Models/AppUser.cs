using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Insta_Blog.Models
{
    public class AppUser:IdentityUser
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string AboutMe { get; set; }
        public string WorkPlace { get; set; }
        public string RelationShip { get; set; }
        public bool Status { get; set; }
        public string Image { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
        public virtual ICollection<LikeCount> PostCounts { get; set; }
        public virtual ICollection<PostCommet> PostCommets { get; set; }


    }
}
