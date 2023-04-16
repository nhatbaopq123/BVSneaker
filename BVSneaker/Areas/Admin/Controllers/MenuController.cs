using System;
using PagedList;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BVSneaker.Models;
using BVSneaker.Models.EF;

namespace BVSneaker.Areas.Admin.Controllers
{
    public class MenuController : Controller
    {
        private ApplicationDbContext _dbConnect = new ApplicationDbContext();
        // GET: Admin/Menu
        public ActionResult Index(string Searchtext, int? page)
        {
            var pageSize = 6;
            if (page == null)
            {
                page = 1;
            }
            IEnumerable<Menu> items = _dbConnect.Menu.OrderByDescending(x => x.Menu_Id);
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
        public ActionResult Add(Menu model)
        {
            if (ModelState.IsValid)
            {
                model.CreateDate = DateTime.Now;
                model.ModDate = DateTime.Now;
                model.Alias = BVSneaker.Models.Common.Filter.FilterChar(model.Title);
                _dbConnect.Menu.Add(model);
                _dbConnect.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }


        public ActionResult Edit(int id)
        {
            var item = _dbConnect.Menu.Find(id);
            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Menu model)
        {
            if (ModelState.IsValid)
            {
                _dbConnect.Menu.Attach(model);
                model.ModDate = DateTime.Now;
                _dbConnect.Entry(model).Property(x => x.Title).IsModified = true;
                _dbConnect.Entry(model).Property(x => x.Description).IsModified = true;
                _dbConnect.Entry(model).Property(x => x.Position).IsModified = true;
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
            var item = _dbConnect.Menu.Find(id);
            if (item != null)
            {
                _dbConnect.Menu.Remove(item);
                _dbConnect.SaveChanges();
                return Json(new { success = true });

            }
            return Json(new { success = false });
        }
    }
}
    
