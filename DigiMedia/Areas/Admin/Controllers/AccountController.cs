using DigiMedia.Core.Models;
using DigiMedia.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DigiMedia.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> CreateAdmin()
        {
            AppUser user = new AppUser
            {
                FullName = "Nicat",
                UserName = "superAdmin",
            };

            await _userManager.CreateAsync(user, "Nicat12@");
            await _userManager.AddToRoleAsync(user, "superAdmin");

            return Ok("Admin yarandi");
        }

        public async Task<IActionResult> CreateRoles()
        {
            IdentityRole identityRole = new IdentityRole("superAdmin");
            IdentityRole identityRole2 = new IdentityRole("Member");
            IdentityRole identityRole3 = new IdentityRole("Admin");

            await _roleManager.CreateAsync(identityRole);
            await _roleManager.CreateAsync(identityRole2);
            await _roleManager.CreateAsync(identityRole3);

            return Ok("Rollar yarandi");
        }


        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(AdminLoginVm adminLoginVm)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            AppUser user = await _userManager.FindByNameAsync(adminLoginVm.UserName);
            if (user == null)
            {
                ModelState.AddModelError("", "UserName or Password is not valid");
                return View();
            }

            var result = await _signInManager.PasswordSignInAsync(user, adminLoginVm.Password, adminLoginVm.IsPersistent, false);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "UserName or Password is not valid");
                return View();
            }

            return RedirectToAction("Index", "Dashboard");
        }

        public async Task<IActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }
    }
}
