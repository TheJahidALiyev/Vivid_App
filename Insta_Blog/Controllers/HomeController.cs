using Insta_Blog.Models;
using Insta_Blog.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Insta_Blog.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, UserManager<AppUser> userManager, AppDbContext context)
        {
            _logger = logger;
            _userManager = userManager;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            AppUser appUser = await _userManager.FindByEmailAsync(User.Identity.Name);
            HomeVM vm = new()
            {
                Posts = _context.Posts.OrderByDescending(x => x.Id).ToList(),
                AppUser = appUser,
                LikeCounts = _context.LikeCounts.ToList(),
                PostCommets = _context.PostCommets.ToList(),
                AppUsers = _context.Users.ToList()
            };
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Comet(HomeVM vm)
        {
            AppUser appUser = await _userManager.FindByEmailAsync(User.Identity.Name);
            PostCommet commet = new PostCommet();
            commet.Commet = vm.Text;
            commet.AppUserId = appUser.Id;
            commet.PostId = vm.TextId;
            await _context.PostCommets.AddAsync(commet);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> LikePost(int? id)
        {
            AppUser appUser = await _userManager.FindByEmailAsync(User.Identity.Name);
            //LikeCount likeCount = await _context.LikeCounts.FindAsync(id);
            List<LikeCount> likeCounts = _context.LikeCounts.Where(x => x.PostId == id).ToList();
            var any = likeCounts.Any(x => x.AppUserId == appUser.Id);
            if (any != true)
            {
                LikeCount lk = new LikeCount();
                lk.AppUserId = appUser.Id; lk.PostId = id;
                await _context.LikeCounts.AddAsync(lk);
                await _context.SaveChangesAsync();
                TempData["Liked"] = "Succefully liked...";
            }
            else
            {
                TempData["Alredy_Liked"] = "Alredy liked...";
            }
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        public async Task<IActionResult> Delete(int id)
        {

            if (id == 0) return NotFound();
            AppUser Finduser = await _userManager.FindByEmailAsync(User.Identity.Name);
            if (Finduser == null) return RedirectToAction("Login", "Auth");
            Post findd = await _context.Posts.FindAsync(id);
            if (findd == null) return NotFound();
            _context.Posts.Remove(findd);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }

    }
}
