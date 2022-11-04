
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
    [Route("ticket")]
    public class TicketController : Controller
    {
        private HelpDeskContext db;

        public TicketController(HelpDeskContext _db)
        {
            this.db = _db;


        }

        [Authorize(Roles = "Employee")]
        [HttpGet]
        [Route("send")]

        public IActionResult Send()
        {
            var ticketViewModel = new TicketViewModel();
            ticketViewModel.ticket = new Ticket();

            var categories = db.Categories.Where(r => r.Status).ToList();
            ticketViewModel.Categories = new SelectList(categories, "Id", "Name");

            var statuses = db.Statuses.Where(r => r.Display).ToList();
            ticketViewModel.Statuses = new SelectList(statuses, "Id", "Name");

            var periods = db.Periods.Where(r => r.Status).ToList();
            ticketViewModel.Periods = new SelectList(periods, "Id", "Name");

            return View("Send", ticketViewModel);
        }


        [HttpPost]
        [Route("send")]

        public IActionResult Send(TicketViewModel ticketViewModel)
        {
            try
            {
                var username = User.FindFirst(ClaimTypes.Name).Value;
                var account = db.Accounts.SingleOrDefault(a => a.UserName.Equals(username));
                ticketViewModel.ticket.CreatedDate = DateTime.Now;
                ticketViewModel.ticket.EmployeeId = account.Id;
                db.Tickets.Add(ticketViewModel.ticket);
                db.SaveChanges();
                TempData["msg"] = "Done";
                ViewBag.msg = "Done";
            }
            catch
            {
                TempData["msg"] = "Failed";
            }
            return RedirectToAction("Send");
        }

        [Authorize(Roles = "Employee")]
        [HttpGet]
        [Route("history")]

        public IActionResult History()
        {
            var username = User.FindFirst(ClaimTypes.Name).Value;
            var account = db.Accounts.SingleOrDefault(a => a.UserName.Equals(username));
            ViewBag.tickets = db.Tickets.OrderByDescending(t => t.Id).Where(t => t.EmployeeId == account.Id).ToList();
            return View("History");



        }


        [Authorize(Roles = "Administrator")]
        [HttpGet]
        [Route("List")]

        public IActionResult List()
        {
          
            ViewBag.tickets = db.Tickets.OrderByDescending(t => t.Id).ToList();
            return View("List");
        }

        [Authorize(Roles = "Administrator")]
        [HttpGet]
        [Route("Assign")]

        public IActionResult Assign()
        {

            ViewBag.tickets = db.Tickets.Where(t => t.Supporter == null).OrderByDescending(t => t.Id).ToList();
            return View("Assign");
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [Route("Assign")]

        public IActionResult Assign(int id, int supporterId)
        {

            var ticket = db.Tickets.Find(id);
            ticket.SupporterId = supporterId;
            db.SaveChanges();
            return RedirectToAction("List");
        }

        //Loaded tickets are assigned to the supporter

        [Authorize(Roles = "Support")]
        [HttpGet]
        [Route("Assigned")]

        public IActionResult Assigned()
        {

            var username = User.FindFirst(ClaimTypes.Name).Value;
            var account = db.Accounts.SingleOrDefault(a => a.UserName.Equals(username));
            ViewBag.tickets = db.Tickets.Where(t => t.SupporterId == account.Id).OrderByDescending(t => t.Id).ToList();
            return View("Assigned");
        }


        [Authorize(Roles = "Administrator,Support,Employee")]
        [HttpGet]
        [Route("Details/{id}")]

        public IActionResult Details(int id)
        {

            ViewBag.tickets = db.Tickets.Find(id);
            ViewBag.supporters = db.Accounts.Where(a => a.RoleId == 2 && a.Status == true).ToList();
            return View("Details");
        }

        



    }

}

