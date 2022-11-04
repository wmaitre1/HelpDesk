
using HelpDesk.Models;
using Microsoft.AspNetCore.Mvc;
using HelpDesk.Security;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Security.Principal;
using System.Security.Claims;
using BCrypt.Net;
using Microsoft.AspNetCore.Authorization;
using HelpDesk.Models.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace HelpDesk.Controllers
{
    [Route("account")]
    public class AccountController : Controller
    {
        private HelpDeskContext db;

        public AccountController(HelpDeskContext _db)
        {
            this.db = _db;


        }

        [Authorize(Roles = "Administrator")]
        [HttpGet]
        [Route("add")]

        public IActionResult Add()
        {


            var accountViewModel = new AccountViewModel();
            accountViewModel.Account = new Account();
            var roles = db.Roles.Where(r => r.Id != 1).ToList();
            accountViewModel.Roles = new SelectList(roles, "Id", "Name");
            return View("Add", accountViewModel);
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [Route("add")]

        public IActionResult Add(AccountViewModel accountViewModel)
        {
            try
            {
                accountViewModel.Account.Password = BCrypt.Net.BCrypt.HashPassword(accountViewModel.Account.Password,
                        BCrypt.Net.BCrypt.GenerateSalt());
                db.Accounts.Add(accountViewModel.Account);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.msg = "Failed";
                return View("Add", accountViewModel);
            }


           
        }


        [Authorize(Roles = "Administrator")]
        [Route("delete /{id}")]

        public IActionResult Delete(int id)
        {
            try
            {
                var account = db.Accounts.SingleOrDefault(a => a.Id == id && a.RoleId != 1);
                db.Accounts.Remove(account);
                db.SaveChanges();
                ViewBag.msg = "Done";
            }
            catch
            {
                ViewBag.msg = "Failed";
               
            }
             var accounts = db.Accounts.Where(a => a.RoleId != 1).ToList();
             return View("Index", accounts);


        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [Route("edit/{id}")]

        public IActionResult Edit(int id, AccountViewModel accountViewModel)
        {
            try
            {
                var password = db.Accounts.AsNoTracking().SingleOrDefault(a => a.Id == id).Password;
                if (!string.IsNullOrEmpty(accountViewModel.Account.Password))
                {
                    password = BCrypt.Net.BCrypt.HashPassword(accountViewModel.Account.Password,
                        BCrypt.Net.BCrypt.GenerateSalt());
                }
                accountViewModel.Account.Password = password;
                db.Entry(accountViewModel.Account).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                ViewBag.msg = "Failed";
                return View("Edit", accountViewModel);
            }



        }

        [Authorize(Roles = "Administrator")]
        [HttpGet]
        [Route("edit/{id}")]

        public IActionResult Edit(int id)
        {


            var accountViewModel = new AccountViewModel();
            accountViewModel.Account = db.Accounts.Find(id);
            var roles = db.Roles.Where(r => r.Id != 1).ToList();
            accountViewModel.Roles = new SelectList(roles, "Id", "Name");
            return View("Edit", accountViewModel);
        }

        [Authorize(Roles = "Administrator")]
        [HttpGet]
        [Route("index")]
        [Route("")]

        public IActionResult Index()
        {
            var accounts = db.Accounts.ToList();
            return View(accounts);



        }

        [Authorize(Roles = "Administrator, Support, Employee")]
        [HttpGet]
        [Route("Profile")]

        public IActionResult Profile()
        {
            var username = User.FindFirst(ClaimTypes.Name).Value;
            var account = db.Accounts.SingleOrDefault(a => a.UserName.Equals(username));
            return View("Index", account);

        }

        [Authorize(Roles = "Administrator, Support, Employee")]
        [HttpPost]
        [Route("Profile")]

        public IActionResult Profile(Account account)
        {
            var username = User.FindFirst(ClaimTypes.Name).Value;
            var currentAccount = db.Accounts.SingleOrDefault(a => a.UserName.Equals(username));
            try
            {

                currentAccount.UserName = account.UserName;
                if (!string.IsNullOrEmpty(account.Password))
                {
                    currentAccount.Password = BCrypt.Net.BCrypt.HashPassword(account.Password, BCrypt.Net.BCrypt.GenerateSalt());
                }
                currentAccount.FullName = account.FullName;
                currentAccount.Email = account.Email;
                db.SaveChanges();
                ViewBag.msg = "Done";
            }
            catch (Exception e)
            {
                ViewBag.msg = "Failed";
            }
            return View("Profile", currentAccount);
        }



    }

    


}

