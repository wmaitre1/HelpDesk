
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
    [Route("category")]
    public class CategoryController : Controller
    {
        private HelpDeskContext db;

        public CategoryController(HelpDeskContext _db)
        {
            this.db = _db;


        }


        [HttpGet]
        [Route("index")]
        [Route("")]

        public IActionResult Index()
        {
            var category = db.Categories.ToList();
            return View("index", category);



        }

        [HttpGet]
        [Route("add")]
        public IActionResult Add()
        {
            return View("Add", new Category());



        }

        [HttpPost]
        [Route("add")]
        public IActionResult Add(Category category)
        {
            try
            {
                db.Categories.Add(category);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                ViewBag.msg = "Failed";
                return View("Add", new Category());
            }



        }

        [HttpGet]
        [Route("delete/{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var category = db.Categories.Find(id);
                db.Categories.Remove(category);
                db.SaveChanges();
                ViewBag.msg = "Done";
                return RedirectToAction("Index");

            }
            catch
            {
                ViewBag.msg = "Failed";
                var category = db.Categories.ToList();
                return View("index", category);

            }





        }

        [HttpGet]
        [Route("edit/{id}")]
        public IActionResult Edit(int id)
        {

            var category = db.Categories.Find(id);
            return View("Edit", category);

        }

        [HttpPost]
        [Route("edit/{id}")]
        public IActionResult Edit(int id, Category category)
        {
            try
            {
                db.Entry(category).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                ViewBag.msg = "Failed";
                return View("Edit", category);
            }

        }

    }    

}


