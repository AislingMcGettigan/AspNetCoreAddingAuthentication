using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WishList.Models;
using WishList.Models.AccountViewModels;

namespace WishList.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult Register(RegisterViewModel viewModel)
        {

            if (!ModelState.IsValid)
            {
                return View("Register", viewModel);
            }

            var appUser = new ApplicationUser()
            {
                UserName = viewModel.Email,
                Email = viewModel.Email,
                PasswordHash = viewModel.Password
            };

            var createUser = _userManager.CreateAsync(appUser).Result;
                if (!createUser.Succeeded)
                {
                    foreach (var error in createUser.Errors)
                    {
                        ModelState.AddModelError("Password", error.Description);
                    }
                    return View("Register", viewModel);
                }

            return RedirectToAction("Home/Index");
        }
    }
}


