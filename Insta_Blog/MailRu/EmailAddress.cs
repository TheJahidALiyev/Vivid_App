using Insta_Blog.Models;
namespace Insta_Blog.MailRu
{
    public class EmailAddress
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public AppUser appUser { get; set; }
    }
}
