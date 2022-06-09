using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskNest.Models;
using TaskNest.Utilies;
using TaskNest.ViewModels.Authorization;

namespace TaskNest.Controllers
{
    
    public class AuthController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AuthController(UserManager<AppUser> userManager, 
                               SignInManager<AppUser> signInManager,
                               RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }
        public IActionResult SignIn()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SignIn(SignInVM signIn,string ReturnUrl)
        {
            AppUser user;
            if (signIn.UsernameOrEmail.Contains("@"))
            {
                user = await _userManager.FindByEmailAsync(signIn.UsernameOrEmail);
            }
            else
            {
                user = await _userManager.FindByNameAsync(signIn.UsernameOrEmail);
            }
            if (user==null)
            {
                ModelState.AddModelError("", "Login ve ya parol yalnisdir");
                return View(signIn);
            }
            var result = await _signInManager.PasswordSignInAsync(user, signIn.Password, signIn.RememberMe, true);
            if (result.IsLockedOut)
            {
                ModelState.AddModelError("", "Sinama cehdini asdiniz. Zehmet olmasa biraz gozleyin");
                return View(signIn);
            }
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Login ve ya parol yalnisdir");
                return View(signIn);
            }
            if (ReturnUrl !=null) return LocalRedirect(ReturnUrl);
            return RedirectToAction("Index", "Home");
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM register)
        {
            if (!ModelState.IsValid) return View();
            AppUser newUser = new AppUser
            {
                Name = register.FirstName,
                Surname = register.LastName,
                Email = register.Email,
                UserName = register.Username
            };
            IdentityResult result = await _userManager.CreateAsync(newUser,register.Password);
            if (!result.Succeeded)
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
            }
            await _userManager.AddToRoleAsync(newUser, UserRoles.Member.ToString());
            return RedirectToAction("Index","Home");
        }
        public async Task<IActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(Register));
        }
        public async Task CreateRoles()
        {
            foreach (var item in Enum.GetValues(typeof(UserRoles)))
            {
                if (!await _roleManager.RoleExistsAsync(item.ToString())) await _roleManager.CreateAsync(new IdentityRole(item.ToString()));
                
            }
        }
        //public IActionResult ResetPassword(ResetPasswordVM resetPasswordVM)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return View(resetPasswordVM);
        //    }
        //}
    }
}
