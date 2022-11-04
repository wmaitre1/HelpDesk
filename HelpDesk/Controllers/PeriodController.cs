
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
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace HelpDesk.Controllers
{
    [Authorize(Roles = "Administrator")]
    [Route("period")]
    public class PeriodController : Controller
    {
        private HelpDeskContext db;

        public PeriodController(HelpDeskContext _db)
        {
            this.db = _db;


        }


        [HttpGet]
        [Route("index")]
        [Route("")]

        public IActionResult Index()
        {
            var period = db.Periods.ToList();
            return View("index", period);



        }

        [HttpGet]
        [Route("add")]
        public IActionResult Add()
        {
            return View("Add", new Period());



        }

        [HttpPost]
        [Route("add")]
        public IActionResult Add(Period period)
        {
            try
            {
                db.Periods.Add(period);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                ViewBag.msg = "Failed";
                return View("Add", new Period());
            }



        }

        [HttpGet]
        [Route("delete/{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var period = db.Periods.Find(id);
                db.Periods.Remove(period);
                db.SaveChanges();
                return RedirectToAction("Index");

            }
            catch
            {
                ViewBag.msg = "Failed";
                var period = db.Periods.ToList();
                return View("index", period);

            }





        }

        [HttpGet]
        [Route("edit/{id}")]
        public IActionResult Edit(int id)
        {

            var period = db.Periods.Find(id);
            return View("Edit", period);

        }

        [HttpPost]
        [Route("edit/{id}")]
        public IActionResult Edit(int id, Period period)
        {
            try
            {
                db.Entry(period).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                ViewBag.msg = "Failed";
                return View("Edit", period);
            }

        }

    }    

}


