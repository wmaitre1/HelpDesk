using HelpDesk.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HelpDesk.Controllers
{
    [Authorize(Roles = "Administrator, Support, Employee")]
    [Route("Dashboard")]
    public class DashboardController : Controller
    {
        //private HelpDeskContext db;
        private readonly HelpDeskContext db;

        public DashboardController(HelpDeskContext _db)
        {
            //this.db = _db;
            db = _db;
        }

        [Route("index")]
        [Route("")]
       
        public IActionResult Index()
        {
            var username = User.FindFirst(ClaimTypes.Name).Value;
            var account = db.Accounts.SingleOrDefault(a => a.UserName.Equals(username));
            ViewBag.tickets = db.Tickets.Where(t => t.EmployeeId == account.Id).ToList();
            return View();
        }
    }
}
