using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Test_SignalR.Data;
using Test_SignalR.Models;

namespace Test_SignalR.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet]
        public IActionResult Login()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        public async Task<IActionResult> Login(User user)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Message = string.Join(",", ModelState.Values.Where(x => x.Errors.Count > 0).SelectMany(x => x.Errors).Select(x => x.ErrorMessage));
                return View(user);
            }
            if (SeedData.Users.Any(u => u.UserName.Equals(user.UserName) && u.Password.Equals(user.Password)))
            {
                var claims = new Claim[]
                {
                    new Claim("UserName", user.UserName),
                    new Claim("ID", user.ID.ToString())
                };
                var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                identity.AddClaims(claims);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

                if (HttpContext.Request.Query["returnurl"].Count > 0)
                {
                    return Redirect(HttpContext.Request.Query["returnurl"].ToString());
                }
                else
                {
                    return RedirectToAction("index", "home");
                }
            }
            else
            {
                ModelState.AddModelError("", "用户名或密码错误");
            }
            return View(user);
        }
    }
}
