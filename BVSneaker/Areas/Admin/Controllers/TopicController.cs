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
    [Authorize(Roles = "Admin,Employee")]
    public class TopicController : Controller
    {
        private ApplicationDbContext _dbConnect = new ApplicationDbContext();
        // GET: Admin/Topic
        public ActionResult Index(string Searchtext, int? page)
        {
            var pageSize = 6;
            if (page == null)
            {
                page = 1;
            }
            IEnumerable<Topic> items = _dbConnect.Topic.OrderByDescending(x => x.Topic_Id);
            if (!string.IsNullOrEmpty(Searchtext))
            {
                items = items.Where(x => x.Alias.Contains(Searchtext) || x.Title.Contains(Searchtext));
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
        public ActionResult Add(Topic model)
        {
            if (ModelState.IsValid)
            {
                model.Alias = BVSneaker.Models.Common.Filter.FilterChar(model.Title);
                model.CreateDate = DateTime.Now;
                model.ModDate = DateTime.Now;
                _dbConnect.Topic.Add(model);
                _dbConnect.SaveChanges();
                return RedirectToAction("Index");

            }
            return View(model);
        }
        public ActionResult Edit(int id)
        {
            var item = _dbConnect.Topic.Find(id);
            return View(item);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Topic model)
        {
            if (ModelState.IsValid)
            {
                _dbConnect.Topic.Attach(model);
                model.ModDate = DateTime.Now;
                model.Alias = BVSneaker.Models.Common.Filter.FilterChar(model.Title);
                _dbConnect.Entry(model).Property(x => x.Title).IsModified = true;
                _dbConnect.Entry(model).Property(x => x.Alias).IsModified = true;
                _dbConnect.Entry(model).Property(x => x.DetailTitle).IsModified = true;
                _dbConnect.Entry(model).Property(x => x.Image).IsModified = true;
                _dbConnect.Entry(model).Property(x => x.Description).IsModified = true;
                _dbConnect.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }
        [HttpPost]
        public ActionResult Delete(int id)
        {
            var item = _dbConnect.Topic.Find(id);
            if (item != null)
            {
                _dbConnect.Topic.Remove(item);
                _dbConnect.SaveChanges();
                return Json(new { success = true });

            }
            return Json(new { success = false });
        }
    }
}