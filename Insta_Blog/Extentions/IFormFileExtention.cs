using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Insta_Blog.Extentions
{
    public static class IFormFileExtention
    {
        //Image
        public static bool IsImage(this IFormFile file)
        {
            return file.ContentType.Contains(@"image/");
        }
        public static bool ImageSize(this IFormFile file, int maxsize)
        {
            return file.Length / 1024 / 1024 < maxsize;
        }
        public async static Task<string> CopyImage(this IFormFile file, string root, string folder)
        {
            string imgpath = Path.Combine(root, "allUserImg");
            string filename = Path.Combine(folder, Guid.NewGuid().ToString() + file.FileName);
            string ResultPath = Path.Combine(imgpath, filename);
            using (FileStream fileStream = new FileStream(ResultPath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }
            string Replaceslahsfilenmae = filename.Replace(@"\", "/");
            return Replaceslahsfilenmae;
        }
        static IFormFile filee { get; set; }
        public async static Task<string> CopyImage2(this string root, string folder)
        {

            string imgpath = Path.Combine(root, "allUserImg");
            string filename = Path.Combine(folder, Guid.NewGuid().ToString());
            string ResultPath = Path.Combine(imgpath, filename);
            using (FileStream fileStream = new FileStream(ResultPath, FileMode.Create))
            {
                await filee.CopyToAsync(fileStream);
            }
            string Replaceslahsfilenmae = filename.Replace(@"\", "/");
            return Replaceslahsfilenmae;
        }
    }
}
