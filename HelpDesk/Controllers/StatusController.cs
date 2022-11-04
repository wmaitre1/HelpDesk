
using HelpDesk.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using HelpDesk.Models.ViewModels;
using Microsoft.EntityFrameworkCore;


namespace HelpDesk.Controllers
{
    [Authorize(Roles = "Administrator")]
    [Route("status")]
    public class StatusController : Controller
    {
        private HelpDeskContext db;

        public StatusController(HelpDeskContext _db)
        {
            this.db = _db;


        }


        [HttpGet]
        [Route("index")]
        [Route("")]

        public IActionResult Index()
        {
            var status = db.Statuses.ToList();
            return View("index", status);



        }

        [HttpGet]
        [Route("add")]
        public IActionResult Add()
        {
            return View("Add", new Status());



        }

        [HttpPost]
        [Route("add")]
        public IActionResult Add(Status status)
        {
            try
            {
                db.Statuses.Add(status);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                ViewBag.msg = "Failed";
                return View("Add", new Status());
            }



        }

        [HttpGet]
        [Route("delete/{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var status = db.Statuses.Find(id);
                db.Statuses.Remove(status);
                db.SaveChanges();
                ViewBag.msg = "Done";
                return RedirectToAction("Index");

            }
            catch
            {
                ViewBag.msg = "Failed";
                var status = db.Statuses.ToList();
                return View("index", status);

            }





        }

        [HttpGet]
        [Route("edit/{id}")]
        public IActionResult Edit(int id)
        {

            var status = db.Statuses.Find(id);
            return View("Edit", status);

        }

        [HttpPost]
        [Route("edit/{id}")]
        public IActionResult Edit(int id, Status status)
        {
            try
            {
                db.Entry(status).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                ViewBag.msg = "Failed";
                return View("Edit", status);
            }

        }

    }    

}


