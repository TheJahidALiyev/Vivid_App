using Microsoft.AspNetCore.Http;

namespace Insta_Blog.Models
{
    public class PostVM
    {
        public string Description { get; set; }
        public IFormFile Photo { get; set; }
        public IFormFile PhotoUpd { get; set; }
    }
}
