using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using theWall.Models;
using theWall.Factory;
using Microsoft.AspNetCore.Identity;

namespace theWall.Controllers
{
public class LoginController : Controller
    {
        private readonly UserFactory userFactory;
        public LoginController(UserFactory user)
        {
            //Instantiate a UserFactory object that is immutable (READONLY)
            //This is establish the initial DB connection for us.
            userFactory = user;
        }
        // GET: /Home/
        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            ViewBag.Errors = "";
            if (TempData["Errors"] != null)
            {
                ViewBag.Errors = TempData["Errors"];
            }
            return View("Index");
        }
        [HttpPost]
        [Route("register")]
        public IActionResult Register(User newUser)
        {
            if (ModelState.IsValid)
            {
                PasswordHasher<User> Hasher = new PasswordHasher<User>();
                newUser.password = Hasher.HashPassword(newUser, newUser.password);
                TryValidateModel(newUser);
                userFactory.Register(newUser);
                System.Console.WriteLine("Validation passed");
                HttpContext.Session.SetInt32("UserId", newUser.id);
                return RedirectToAction("Dashboard", "Message");
            }
            else
            {
                // Errors list creation for storage in TempData to be passed to index method
                List<string> Errors = new List<string>();
                System.Console.WriteLine(ModelState.Values);
                foreach (var error in ModelState.Values)
                {
                    // Checks if there is Errors in error?
                    if (error.Errors.Count > 0)
                    {
                        Errors.Add(error.Errors[0].ErrorMessage);
                    }
                }
                TempData["Errors"] = Errors;
                return RedirectToAction("Index");
            }
        }
        [HttpPost]
        [Route("login")]
        public IActionResult Login(string email, string password)
        {
            if (email != null && password != null)
            {
                User user = userFactory.Login(email);
                if (user != null)
                {
                    var Hasher = new PasswordHasher<User>();
                    if (0 != Hasher.VerifyHashedPassword(user, user.password, password))
                    {
                        HttpContext.Session.SetInt32("UserId", user.id);
                        return RedirectToAction("Dashboard", "Message");
                    }
                }
            }
            List<string> Errors = new List<string>();
            Errors.Add("Invalid email or password");
            TempData["Errors"] = Errors;
            return RedirectToAction("Index");
        }
        [HttpGet]
        [Route("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }
    }
}
