using Drugstore.Identity;
using Drugstore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Drugstore.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly UserManager<SystemUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly SignInManager<SystemUser> signInManager;
        private readonly IServiceProvider serviceProvider;

        public AccountController(
            UserManager<SystemUser> userManager,
            RoleManager<IdentityRole> roleManager,
            SignInManager<SystemUser> signInManager,
            IServiceProvider serviceProvider)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.signInManager = signInManager;
            this.serviceProvider = serviceProvider;
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }


        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult Login(LoginViewModel details)
        {
            if (ModelState.IsValid)
            {
                SystemUser user = userManager.FindByEmailAsync(details.Email).Result ??
                    userManager.FindByNameAsync(details.Email).Result;
                if (user != null)
                {

                    signInManager.SignOutAsync().Wait();
                    var result = signInManager
                        .PasswordSignInAsync(user, details.Password, false, false)
                        .Result;
                    if (result.Succeeded)
                    {
                        return RedirectAfterLogin(user);
                    }
                }
                ModelState.AddModelError(nameof(LoginViewModel.Email),
                "Nieprawidłowa nazwa użytkownika lub hasło.");
            }
            return View(details);
        }

        [HttpGet]
        [AllowAnonymous]
        private IActionResult RedirectAfterLogin(SystemUser user)
        {
            var role = userManager.GetRolesAsync(user)
                .Result.FirstOrDefault();
            if (!string.IsNullOrEmpty(role))
            {
                return RedirectToAction("Index", role);
            }

            return RedirectToAction("Login");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Redirect()
          {
            var user = userManager.GetUserAsync(User).Result;

            if (user != null)
            {
                var role = userManager.GetRolesAsync(user)
                    .Result.FirstOrDefault();
                if (!string.IsNullOrEmpty(role))
                {
                    return RedirectToAction("Index", role);
                }
            }

            return RedirectToAction("Login");
        }


        [AllowAnonymous]
        public ViewResult AccessDenied() => View();
    }
}
