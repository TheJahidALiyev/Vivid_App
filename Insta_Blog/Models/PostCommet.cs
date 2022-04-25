namespace Insta_Blog.Models
{
    public class PostCommet
    {
        public int Id { get; set; }
        public string AppUserId { get; set; }
        public virtual AppUser AppUser { get; set; }
        public int? PostId { get; set; }
        public virtual Post Post { get; set; }
        public string Commet { get; set; }
    }
}
