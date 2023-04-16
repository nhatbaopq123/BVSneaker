using BVSneaker.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BVSneaker.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RoleController : Controller
    {
        private ApplicationDbContext _dbConnect = new ApplicationDbContext();
        // GET: Admin/Role
        public ActionResult Index()
        {
            var item = _dbConnect.Roles.ToList();
            return View(item);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IdentityRole model)
        {
            if (ModelState.IsValid)
            {
                var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(_dbConnect));
                roleManager.Create(model);
                return RedirectToAction("Index");
            }
            return View(model);
        }
        public ActionResult Edit(string id)
        {
            var item = _dbConnect.Roles.Find(id);
            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(IdentityRole model)
        {
            if (ModelState.IsValid)
            {
                var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(_dbConnect));
                roleManager.Update(model);
                return RedirectToAction("Index");
            }
            return View(model);
        }
    }
}