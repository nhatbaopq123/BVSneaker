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
    public class BrandController : Controller
    {
        private ApplicationDbContext _dbConnect = new ApplicationDbContext();
        // GET: Admin/Brand
        public ActionResult Index(string Searchtext, int? page)
        {
            var pageSize = 6;
            if (page == null)
            {
                page = 1;
            }
            IEnumerable<Brand> items = _dbConnect.Brand.OrderByDescending(x => x.Brand_Id);
            if (!string.IsNullOrEmpty(Searchtext))
            {
                items = items.Where(x => x.Alias.Contains(Searchtext) || x.Name.Contains(Searchtext));
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
        public ActionResult Add(Brand model)
        {
            if (ModelState.IsValid)
            {
                model.Alias = BVSneaker.Models.Common.Filter.FilterChar(model.Name);
                _dbConnect.Brand.Add(model);
                _dbConnect.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }
        public ActionResult Edit(int id)
        {
            var item = _dbConnect.Brand.Find(id);
            return View(item);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Brand model)
        {
            if (ModelState.IsValid)
            {
                _dbConnect.Brand.Attach(model);
                model.Alias = BVSneaker.Models.Common.Filter.FilterChar(model.Name);
                _dbConnect.Entry(model).Property(x => x.Name).IsModified = true;
                _dbConnect.Entry(model).Property(x => x.Image).IsModified = true;
                _dbConnect.Entry(model).Property(x => x.Alias).IsModified = true;
                _dbConnect.Entry(model).Property(x => x.Email).IsModified = true;
                _dbConnect.Entry(model).Property(x => x.Address).IsModified = true;
                _dbConnect.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }
        [HttpPost]
        public ActionResult Delete(int id)
        {
            var item = _dbConnect.Brand.Find(id);
            if (item != null)
            {
                _dbConnect.Brand.Remove(item);
                _dbConnect.SaveChanges();
                return Json(new { success = true });

            }
            return Json(new { success = false });
        }
    }
}