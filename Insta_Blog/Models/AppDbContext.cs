using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Insta_Blog.Models
{
    public class AppDbContext:IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
        {

        }
        public virtual DbSet<Post> Posts { get; set; }   
        public virtual DbSet<LikeCount> LikeCounts { get; set; }
        public virtual DbSet<PostCommet> PostCommets { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Data Source=SQL8001.site4now.net;Initial Catalog=db_a8604f_senanblogdb;User Id=db_a8604f_senanblogdb_admin;Password=Senan@321");
                //optionsBuilder.UseSqlServer("Server=sql5053.site4now.net;Database=db_a764a4_anar83;User Id=db_a764a4_anar83_admin;password=qxj2Rz2h1uN6;Trusted_Connection=False;MultipleActiveResultSets=true;");
                // "DefaultConnection": "Server=servername;Database=dbnaem;User Id=username;password=password;Trusted_Connection=False;MultipleActiveResultSets=true;"
            }
        }
    }
}
