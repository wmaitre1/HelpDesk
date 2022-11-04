
using HelpDesk.Models;
using Microsoft.AspNetCore.Mvc;
using HelpDesk.Security;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Security.Principal;

namespace HelpDesk.Controllers
{
    [Route("logIn")]
    public class LogInController : Controller
    {
        private HelpDeskContext db;

        public LogInController(HelpDeskContext _db)
        {
            this.db = _db;
        }

        [Route("index")]
        [Route("")]
        [Route("~/")]
        public IActionResult Index()
        {
                  
            return View();
        }

        [HttpPost]
        [Route("process")]
        public IActionResult process(string username, string password)
        {
            var account = check(username, password);
            if (account != null)
            {
                SecurityManager securityManager = new SecurityManager();
                securityManager.SignIn(HttpContext, account);
                return RedirectToAction("Index", "Dashboard");
            }
            else
            {
                ViewBag.error = "Invalid";
                return View("Index");
            }
            
        }

        [Route("SignOut")]

        public IActionResult SignOut()
        {
            SecurityManager securityManager = new SecurityManager();
            securityManager.SignOut(HttpContext);
            return RedirectToAction("Index");
        }

        [Route("AccessDenied")]

        public IActionResult AccessDenied()
        {
            
            return View("AccessDenied");
        }

        private Account check(string username, string password)
        {
            var account = db.Accounts.SingleOrDefault(a => a.UserName.Equals(username) && a.Status == true);
            if (account != null)
            {
                if (BCrypt.Net.BCrypt.Verify(password, account.Password))
                {
                    return account;
                }
            }
            return null;
        }
    }
}
