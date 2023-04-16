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
    public class AdvertisementController : Controller
    {
        private ApplicationDbContext _dbConnect = new ApplicationDbContext();
        // GET: Admin/Advertisement
        public ActionResult Index(string Searchtext, int? page)
        {
            var pageSize = 6;
            if (page == null)
            {
                page = 1;
            }
            IEnumerable<Advertisement> items = _dbConnect.Advertisements.OrderByDescending(x => x.Adv_Id);
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
        public ActionResult Add(Advertisement model)
        {
            if (ModelState.IsValid)
            {
                if (model.Alias == null)
                {
                    model.Alias = BVSneaker.Models.Common.Filter.FilterChar(model.Title);
                }
                model.CreateDate = DateTime.Now;
                model.ModDate = DateTime.Now;
                _dbConnect.Advertisements.Add(model);
                _dbConnect.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }
        public ActionResult Edit(int id)
        {
            var item = _dbConnect.Advertisements.Find(id);
            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Advertisement model)
        {
            if (ModelState.IsValid)
            {
                _dbConnect.Advertisements.Attach(model);
                model.ModDate = DateTime.Now;
                if (model.Alias == null)
                {
                    model.Alias = BVSneaker.Models.Common.Filter.FilterChar(model.Title);
                }
                _dbConnect.Entry(model).Property(x => x.Title).IsModified = true;
                _dbConnect.Entry(model).Property(x => x.Description).IsModified = true;
                _dbConnect.Entry(model).Property(x => x.Alias).IsModified = true;
                _dbConnect.Entry(model).Property(x => x.Image).IsModified = true;
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
            var item = _dbConnect.Advertisements.Find(id);
            if (item != null)
            {
                _dbConnect.Advertisements.Remove(item);
                _dbConnect.SaveChanges();
                return Json(new { success = true });

            }
            return Json(new { success = false });
        }
    }
}