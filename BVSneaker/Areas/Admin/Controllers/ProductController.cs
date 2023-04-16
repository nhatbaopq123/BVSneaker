using BVSneaker.Models.EF;
using BVSneaker.Models;
using PagedList;
using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BVSneaker.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin,Employee")]
    public class ProductController : Controller
    {
        private ApplicationDbContext _dbConnect = new ApplicationDbContext();
        // GET: Admin/Product
        public ActionResult Index(string Searchtext, int? page)
        {
            IEnumerable<Product> items = _dbConnect.Product.OrderByDescending(x => x.Product_Id).Include(s => s.ProductStock);
            var pageSize = 6;
            if (page == null)
            {
                page = 1;
            }
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
            ViewBag.Topic = new SelectList(_dbConnect.Topic.ToList(), "Topic_Id", "Title");
            ViewBag.Brand = new SelectList(_dbConnect.Brand.ToList(), "Brand_Id", "Name");
            ViewBag.Category = new SelectList(_dbConnect.Category.ToList(), "Category_Id", "Name");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(Product model, List<string> Images, List<int> rDefault,
            List<int> Quantity, List<int> Size, List<decimal> Price)
        {
            if (ModelState.IsValid)
            {
                if (Images != null && Images.Count > 0)
                {
                    for (int i = 0; i < Images.Count; i++)
                    {
                        if (i + 1 == rDefault[0])
                        {
                            model.ProductImage.Add(new ProductImage
                            {
                                Product_Id = model.Product_Id,
                                Image = Images[i],
                                isDefault = true

                            });
                        }
                        else
                        {
                            model.ProductImage.Add(new ProductImage
                            {
                                Product_Id = model.Product_Id,
                                Image = Images[i],
                                isDefault = false

                            });
                        }
                    }
                }
                model.CreateDate = DateTime.Now;
                model.ModDate = DateTime.Now;
                model.Alias = BVSneaker.Models.Common.Filter.FilterChar(model.Name);
                _dbConnect.Product.Add(model);
                _dbConnect.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Topic = new SelectList(_dbConnect.Topic.ToList(), "Topic_Id", "Title");
            ViewBag.Brand = new SelectList(_dbConnect.Brand.ToList(), "Brand_Id", "Name");
            ViewBag.Category = new SelectList(_dbConnect.Category.ToList(), "Category_Id", "Name");
            return View(model);
        }

        public ActionResult Detail(int id)
        {
            ViewBag.Topic = new SelectList(_dbConnect.Topic.ToList(), "Topic_Id", "Title");
            ViewBag.Brand = new SelectList(_dbConnect.Brand.ToList(), "Brand_Id", "Name");
            ViewBag.Category = new SelectList(_dbConnect.Category.ToList(), "Category_Id", "Name");
            var product = _dbConnect.Product
                    .Include(s => s.ProductStock)
                    .SingleOrDefault(p => p.Product_Id == id);
            return View(product);
        }
        public ActionResult Edit(int id)
        {
            ViewBag.Topic = new SelectList(_dbConnect.Topic.ToList(), "Topic_Id", "Title");
            ViewBag.Brand = new SelectList(_dbConnect.Brand.ToList(), "Brand_Id", "Name");
            ViewBag.Category = new SelectList(_dbConnect.Category.ToList(), "Category_Id", "Name");
            var item = _dbConnect.Product.Find(id);
            return View(item);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Product model)
        {
            if (ModelState.IsValid)
            {
                model.ModDate = DateTime.Now;
                model.Alias = BVSneaker.Models.Common.Filter.FilterChar(model.Name);
                _dbConnect.Product.Attach(model);
                _dbConnect.Entry(model).State = System.Data.Entity.EntityState.Modified;
                _dbConnect.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Topic = new SelectList(_dbConnect.Topic.ToList(), "Topic_Id", "Title");
            ViewBag.Brand = new SelectList(_dbConnect.Brand.ToList(), "Brand_Id", "Name");
            ViewBag.Category = new SelectList(_dbConnect.Category.ToList(), "Category_Id", "Name");
            return View(model);
        }
        public ActionResult Delete(int id)
        {
            var item = _dbConnect.Product.Find(id);
            if (item != null)
            {
                _dbConnect.Product.Remove(item);
                _dbConnect.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }

        [HttpPost]
        public ActionResult IsHome(int id)
        {
            var item = _dbConnect.Product.Find(id);
            if (item != null)
            {
                item.isHome = !item.isHome;
                _dbConnect.Entry(item).State = System.Data.Entity.EntityState.Modified;
                _dbConnect.SaveChanges();
                return Json(new { success = true, isHome = item.isHome });
            }
            return Json(new { success = false });
        }

        [HttpPost]
        public ActionResult IsHot(int id)
        {
            var item = _dbConnect.Product.Find(id);
            if (item != null)
            {
                item.isHot = !item.isHot;
                _dbConnect.Entry(item).State = System.Data.Entity.EntityState.Modified;
                _dbConnect.SaveChanges();
                return Json(new { success = true, isHot = item.isHot });
            }
            return Json(new { success = false });
        }

        [HttpPost]
        public ActionResult IsBestSeller(int id)
        {
            var item = _dbConnect.Product.Find(id);
            if (item != null)
            {
                item.isBestSeller = !item.isBestSeller;
                _dbConnect.Entry(item).State = System.Data.Entity.EntityState.Modified;
                _dbConnect.SaveChanges();
                return Json(new { success = true, isBestSeller = item.isBestSeller });
            }
            return Json(new { success = false });
        }
    }
}