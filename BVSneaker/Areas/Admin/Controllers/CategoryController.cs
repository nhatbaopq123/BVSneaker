using BVSneaker.Models.EF;
using BVSneaker.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BVSneaker.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CategoryController : Controller
    {
        private ApplicationDbContext _dbConnect = new ApplicationDbContext();
        // GET: Admin/Category
        public ActionResult Index(string Searchtext, int? page)
        {
            var pageSize = 6;
            if (page == null)
            {
                page = 1;
            }
            IEnumerable<Category> items = _dbConnect.Category.OrderByDescending(x => x.Category_Id);
            if (!string.IsNullOrEmpty(Searchtext))
            {
                items = items.Where(x => x.Name.Contains(Searchtext));
            }
            var pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            items = items.ToPagedList(pageIndex, pageSize);
            ViewBag.PageSize = pageSize;
            ViewBag.Page = page;
            return View(items);
        }
        public ActionResult Add()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(Category model)
        {
            if (ModelState.IsValid)
            {
                model.CreateDate = DateTime.Now;
                model.ModDate = DateTime.Now;
                _dbConnect.Category.Add(model);
                _dbConnect.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }
        public ActionResult Edit(int id)
        {
            var item = _dbConnect.Category.Find(id);
            return View(item);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Category model)
        {
            if (ModelState.IsValid)
            {
                _dbConnect.Category.Attach(model);
                model.ModDate = DateTime.Now;
                _dbConnect.Entry(model).Property(x => x.Name).IsModified = true;
                _dbConnect.Entry(model).Property(x => x.ModBy).IsModified = true;
                _dbConnect.Entry(model).Property(x => x.ModDate).IsModified = true;
                _dbConnect.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }
        [HttpPost]
        public ActionResult Delete(int id)
        {
            var item = _dbConnect.Category.Find(id);
            if (item != null)
            {
                _dbConnect.Category.Remove(item);
                _dbConnect.SaveChanges();
                return Json(new { success = true });

            }
            return Json(new { success = false });
        }
    }
}