using System.IO;

namespace Insta_Blog.Utilities
{
    public static class Utility
    {
        public enum Roles
        {

            Member,
            Admin,
            Moderator,

        }
        public const string ModeratorRole = "Moderator";
        public const string MemberRole = "Member";
        public const string AdminRole = "Admin";
        public static bool DeleteImageFromFolder(this string root, string image)
        {
            string path = Path.Combine(root, "assets", image);
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
                return true;
            }
            return false;
        }  
    }
}
