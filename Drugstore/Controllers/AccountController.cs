using Drugstore.Identity;
using Drugstore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Drugstore.Controllers
{
    [Authorize]
    [Route("[controller]/[action]")]
    public class AccountController :Controller
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


        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Login",null);
        }


        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string returnUrl)
        {
            IdentityDataSeeder.Initialize(serviceProvider);
            ViewBag.returnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginModel details, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                SystemUser user = await userManager.FindByEmailAsync(details.Email) ??
                    await userManager.FindByNameAsync(details.Email);
                if (user != null)
                {
                    var role = userManager.GetRolesAsync(user)
                        .Result
                        .FirstOrDefault();

                    await signInManager.SignOutAsync();
                    var result = await signInManager
                        .PasswordSignInAsync(user, details.Password, false, false);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index", role);
                    }
                }
                ModelState.AddModelError(nameof(LoginModel.Email),
                "Nieprawidłowa nazwa użytkownika lub hasło.");
            }
            return View(details);
        }


        [AllowAnonymous]
        public ViewResult AccessDenied() => View();
    }
}
