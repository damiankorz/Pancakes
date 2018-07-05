using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Activities.Models;

namespace Activities.Controllers
{
    public class UserController : Controller
    {
        private ActivityContext _context;
        public UserController(ActivityContext context)
        {
            _context = context;
        }
        // GET: /
        [HttpGet("")]
        public IActionResult Index()
        {
            if(HttpContext.Session.GetInt32("id") == null)
            {
                return View();
            }
            return RedirectToAction("Index", "Home");
        }
        // POST: /login 
        [HttpPost("login")]
        public IActionResult Login(UserViewModels model)
        {
            if(ModelState.IsValid)
            {
                List<User> users = _context.Users.Where(u => u.Email == model.Login.LoginEmail).ToList();
                if(users.Count == 0)
                {
                    ModelState.AddModelError("LoginEmail", "Incorrect email/password combination");
                    return View("Index", model);
                }
                else
                {
                    PasswordHasher<UserViewModels> hasher = new PasswordHasher<UserViewModels>();
                    string hashedPassword = users[0].Password;
                    PasswordVerificationResult result = hasher.VerifyHashedPassword(model, hashedPassword, model.Login.LoginPassword);
                    if(result == PasswordVerificationResult.Failed)
                    {
                        ModelState.AddModelError("LoginPassword", "Incorrect email/password combination");
                        return View("Index", model);
                    }
                    else
                    {
                        HttpContext.Session.SetInt32("id", users[0].ID);
                        return RedirectToAction("Index", "Home");
                    }
                }
            }
            return View("Index", model);
        }
        // POST: /register
        [HttpPost("register")]
        public IActionResult Register(UserViewModels model)
        {
            if(ModelState.IsValid)
            {
                List<User> users = _context.Users.Where(u => u.Email == model.Register.Email).ToList();
                if(users.Count > 0)
                {
                    ModelState.AddModelError("Email", "Email already exists. Please select a unique email");
                    return View("Index", model);
                }
                PasswordHasher<UserViewModels> hasher = new PasswordHasher<UserViewModels>();
                string hashedPassword = hasher.HashPassword(model, model.Register.Password);
                User newUser = new User
                {
                    FirstName = model.Register.FirstName,
                    LastName = model.Register.LastName,
                    Email = model.Register.Email,
                    Password = hashedPassword
                };
                _context.Add(newUser);
                _context.SaveChanges();
                HttpContext.Session.SetInt32("id", newUser.ID);
                return RedirectToAction("Index", "Home");
            }
            return View("Index", model);
        }
    }
}
