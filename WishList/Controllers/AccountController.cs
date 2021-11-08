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

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View("Login", viewModel);
            }

            var signIn = _signInManager.PasswordSignInAsync(viewModel.Email, viewModel.Password, false,false).Result;

            if (!signIn.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            }

            return RedirectToAction("Item/Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult Logout()
        {

            _signInManager.SignOutAsync();

            return RedirectToAction("Home/Index");
        }
}
}


