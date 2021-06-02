using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using System;
using WebProject.Models;
using WebProject.Services;

namespace WebProject.Controllers
{
    [Route("User")]
    public class UserController : Controller
    {
        private readonly IUsersService _userService;
        public UserController(IUsersService service) => this._userService = service;
        [HttpGet("Signup")]
        public IActionResult Signup()// go to sign up page
        {
            ViewData["CurrentPartialView"] = "SignUpPage";
            return View("Views/Home/Index.cshtml");
        }
        [HttpPost("Signup_Submit")]
        public IActionResult Signup_Submit(UserModel user)//signup user 
        {
            if (_userService.AddUser(user))
            {
                var options = new CookieOptions
                {
                    Expires = DateTime.Now.AddHours(1)
                };
                HttpContext.Response.Cookies.Append("UserName", user.UserName, options);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ErrorViewModel error = new ErrorViewModel();
                error.RequestId = "The user is already exists";
                return View("Views/Shared/Error.cshtml", error);
            }
        }
        [HttpPost("Update_Submit")]//update user
        public IActionResult Update_Submit(UserModel user)
        {
            user.UserName = HttpContext.Request.Cookies["UserName"];
            _userService.UpdateUser(user);
            return RedirectToAction("Signup");
        }
        [HttpPost("LogIn")]//log in page submit
        public IActionResult LogIn(UserModel user)
        {
            if (_userService.LogIn(user))
            {
                var options = new CookieOptions
                {
                    Expires = DateTime.Now.AddHours(1)
                };
                HttpContext.Response.Cookies.Append("UserName", user.UserName, options);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ErrorViewModel error = new ErrorViewModel();
                error.RequestId = "There was a problem in the login";
                return View("Views/Shared/Error.cshtml", error);
            }
        }
    }
}