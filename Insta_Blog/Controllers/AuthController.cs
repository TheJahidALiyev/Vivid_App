using Insta_Blog.Extentions;
using Insta_Blog.Models;
using Insta_Blog.Utilities;
using Insta_Blog.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
 

namespace Insta_Blog.Controllers
{
    public class AuthController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IEmailSender _emailsender;
        private readonly IHostingEnvironment _env;
        public AuthController(IHostingEnvironment env,
             AppDbContext context,
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            RoleManager<IdentityRole> roleManager, IEmailSender emailSender
            )
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _emailsender = emailSender;
            _env = env;
        }


        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM loginViewModel)
        {
            if (!ModelState.IsValid) return View(loginViewModel);
            AppUser user = await _userManager.FindByEmailAsync(loginViewModel.Email);
            if (user == null)
            {
                ModelState.AddModelError("", "E-mail or password is wrong.");
                return View(loginViewModel);
            }

            if (!user.EmailConfirmed)
            {
                ModelState.AddModelError("", "Your e-mail address is not confirmed.Please check your e-mail.");
                return View(loginViewModel);
            }

            Microsoft.AspNetCore.Identity.SignInResult signInResult = await _signInManager.PasswordSignInAsync(user, loginViewModel.Password, loginViewModel.RememberMe, true);
            if (!signInResult.Succeeded)
            {
                ModelState.AddModelError("", "E-mail or password is wrong.");
                return View(loginViewModel);
            }

            return RedirectToAction("Index", "Home");

        }
        public IActionResult SignUp()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignUp(SignUpVM signUpVM)
        {
            if (!ModelState.IsValid) return View(signUpVM);
            AppUser newUser = new AppUser()
            {

                Email = signUpVM.Email,
                UserName = signUpVM.Email,
                Name=signUpVM.Name,
                Surname=signUpVM.Surname,  
                Image= "boy-512.png"
            };

            IdentityResult identityResult = await _userManager.CreateAsync(newUser, signUpVM.Password);

            if (!identityResult.Succeeded)
            {
                foreach (var error in identityResult.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(signUpVM);
            }

            try
            {
                string token = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);

                byte[] tokenGeneratedBytes = Encoding.UTF8.GetBytes(token);
                var codeEncoded = WebEncoders.Base64UrlEncode(tokenGeneratedBytes);
                await _emailsender.SendEmailAsync(signUpVM.Email, "Confirm your e-mail address",
                    $"Follow this link for confirming " +
                    $"<a href='{HtmlEncoder.Default.Encode($"http://senansafarali-001-site1.itempurl.com/Auth/ConfirmEmail?token={codeEncoded}&userId={newUser.Id}")}'>" +
                    $"  Link" +
                    $"</a>"
                    );

                await _userManager.AddToRoleAsync(newUser, Utility.Roles.Member.ToString());
                await _signInManager.SignInAsync(newUser, true);
            }
            catch ( Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            ViewData["Gmail"] = newUser.Email+ " check your E-mail address.";
            return View("Login");
        }
        public IActionResult ForgotPassword()
        {
            return View();
        }

        public async Task< IActionResult >Edit() 
        {
            //AppUser appUser = await _userManager.FindByEmailAsync(User.Identity.Name);
            //if(appUser == null) return NotFound();
            //return View(appUser);
            HrInformationImageVM ımageVM = new HrInformationImageVM
            {
                AppUser = await _userManager.FindByEmailAsync(User.Identity.Name),
            };
            if (ımageVM == null) NotFound();
            return View(ımageVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UploadPost(HomeVM vm)
        {
            AppUser findUser = await _userManager.FindByEmailAsync(User.Identity.Name);

            Post post = new Post()
            {
                Description = null,
                ExecTime=DateTime.Now,
                AppUserId=findUser.Id,

            };

            if (vm.Photo != null)
            {
                if (!vm.Photo.IsImage())
                {
                    ModelState.AddModelError("Photo", "Zəhmət olmasa şəkil faylını düzgün seçin");
                    return View(vm);
                }

                if (!vm.Photo.ImageSize(20))
                {
                    ModelState.AddModelError("Photo", "Şəkil faylının ölçüsü 20Mb-dan artıq ola bilməz");
                    return View(vm);
                }
                string filenamePhoto = await vm.Photo.CopyImage(_env.WebRootPath, "");
                post.ImageUrl = filenamePhoto;
            }

            await _context.Posts.AddAsync(post);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index","Home");

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(HrInformationImageVM updateAppUser, IFormFile Image)
        {
            
            AppUser appUser = await _userManager.FindByEmailAsync(User.Identity.Name);
             
            HrInformationImageVM vM = new HrInformationImageVM
            {
                AppUser = await _userManager.FindByEmailAsync(User.Identity.Name),
            };

            if (vM == null)NotFound();

            vM.AppUser.Name = updateAppUser.AppUser.Name.Trim();
            vM.AppUser.Surname = updateAppUser.AppUser.Surname.Trim();
            vM.AppUser.Email = updateAppUser.AppUser.Email.Trim();
            vM.AppUser.RelationShip = updateAppUser.AppUser.RelationShip;
            vM.AppUser.AboutMe = updateAppUser.AppUser.AboutMe.Trim();
            vM.AppUser.WorkPlace = updateAppUser.AppUser.WorkPlace.Trim();

            if (Image != null)
            {
                if (!Image.IsImage())
                {
                    //ModelState.AddModelError("Image", "Şəkil faylı seçin");
                    ViewData["Image_error"] = "Şəkil faylı seçin";
                    return View(vM);
                }

                if (!Image.ImageSize(20))
                {
                    //ModelState.AddModelError("Image", "Şəkilin ölçüsü 2Mb-dan artıq ola bilməz");
                    ViewData["Image_error_size"] = "Şəkilin ölçüsü 2Mb-dan artıq ola bilməz";
                    return View(vM);
                }
                if (vM.AppUser.Image != null)
                {
                    bool deletePicture = Utility.DeleteImageFromFolder(_env.WebRootPath, vM.AppUser.Image);
                    string filename = await Image.CopyImage(_env.WebRootPath, "");
                    vM.AppUser.Image = filename;
                    
                }
            }

            await _userManager.UpdateAsync(vM.AppUser);
            return RedirectToAction(nameof(Edit));





            
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel forgotPassword)
        {
            var user = await _userManager.FindByEmailAsync(forgotPassword.Email);
            if (user == null)
            {
                //ViewBag.Error = "Daxil etdiyiniz e-poçt mövcud deyil";
                ModelState.AddModelError("", "Entered mail can't found.");
                return View();
            }
            string token = await _userManager.GeneratePasswordResetTokenAsync(user);
            byte[] tokenGeneratedBytes = Encoding.UTF8.GetBytes(token);
            var codeEncoded = WebEncoders.Base64UrlEncode(tokenGeneratedBytes);
            await _emailsender.SendEmailAsync(forgotPassword.Email, "Reset Password",
                $"Reset password following this link" +
                $"<a href='{HtmlEncoder.Default.Encode($"http://senansafarali-001-site1.itempurl.com/Auth/ResetPassword?token={codeEncoded}&userId={user.Id}")}'>" +
                $"  Link" +
                $"</a>"
                );
            ViewData["Gmail"] = user.Email + " check your E-mail address.";
            return View("Login");

        }

        public IActionResult ResetPassword(string token, string userId)
        {
            var model = new UserResetPasswordModel
            {
                Token = token,
                UserId = userId
            };
            return View(model);
        }

        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                var codeDecodedBytes = WebEncoders.Base64UrlDecode(token);
                var codeDecoded = Encoding.UTF8.GetString(codeDecodedBytes);
                IdentityResult result = await _userManager.ConfirmEmailAsync(user, codeDecoded);
                if (result.Succeeded)
                {

                    return View();
                }
            }
            return View("FailedConfirmation");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(UserResetPasswordModel userReset)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "New password is not right!");
                return View();
            }

            var user = await _userManager.FindByIdAsync(userReset.UserId);
            if (user == null)
            {
                ModelState.AddModelError("", "User wasn't found");
                return View();
            }

            var codeDecodedBytes = WebEncoders.Base64UrlDecode(userReset.Token);
            var codeDecoded = Encoding.UTF8.GetString(codeDecodedBytes);

            var result = await _userManager.ResetPasswordAsync(user, codeDecoded, userReset.Password);
            if (!result.Succeeded)
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
                return View(userReset);
            }
            return RedirectToAction("Login");
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Auth");
        }
        #region RoleSeeder
        public async Task RoleSeeder()
        {
            if (!await _roleManager.RoleExistsAsync(Utility.Roles.Admin.ToString()))
            {
                await _roleManager.CreateAsync(new IdentityRole(Utility.Roles.Admin.ToString()));
            }


            if (!await _roleManager.RoleExistsAsync(Utility.Roles.Member.ToString()))
            {
                await _roleManager.CreateAsync(new IdentityRole(Utility.Roles.Member.ToString()));
            }
            if (!await _roleManager.RoleExistsAsync(Utility.Roles.Moderator.ToString()))
            {
                await _roleManager.CreateAsync(new IdentityRole(Utility.Roles.Moderator.ToString()));
            }


        }



        #endregion
    }
}
