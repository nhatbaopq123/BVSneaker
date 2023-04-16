using BVSneaker.Models;
using PagedList;
using BVSneaker.Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BVSneaker.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin,Employee")]
    public class BlogController : Controller
    {
        private ApplicationDbContext _dbConnect = new ApplicationDbContext();
        // GET: Admin/Blog
        public ActionResult Index(string Searchtext, int? page)
        {
            var pageSize = 6;
            if (page == null)
            {
                page = 1;
            }
            IEnumerable<Blog> items = _dbConnect.Blog.OrderByDescending(x => x.Blog_Id);
            if (!string.IsNullOrEmpty(Searchtext))
            {
                items = items.Where(x => x.Title.Contains(Searchtext));
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
        public ActionResult Add(Blog model)
        {
            if (ModelState.IsValid)
            {
                model.CreateDate = DateTime.Now;
                model.ModDate = DateTime.Now;
                _dbConnect.Blog.Add(model);
                _dbConnect.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }
        public ActionResult Edit(int id)
        {
            var item = _dbConnect.Blog.Find(id);
            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Blog model)
        {
            if (ModelState.IsValid)
            {
                _dbConnect.Blog.Attach(model);
                model.ModDate = DateTime.Now;
                _dbConnect.Entry(model).Property(x => x.Title).IsModified = true;
                _dbConnect.Entry(model).Property(x => x.Content).IsModified = true;
                _dbConnect.Entry(model).Property(x => x.Description).IsModified = true;
                _dbConnect.Entry(model).Property(x => x.Author).IsModified = true;
                _dbConnect.Entry(model).Property(x => x.CoverImage).IsModified = true;
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
            var item = _dbConnect.Blog.Find(id);
            if (item != null)
            {
                _dbConnect.Blog.Remove(item);
                _dbConnect.SaveChanges();
                return Json(new { success = true });

            }
            return Json(new { success = false });
        }
    }
}