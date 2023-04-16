using BVSneaker.Models.EF;
using BVSneaker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BVSneaker.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin,Employee")]
    public class ProductStockController : Controller
    {
        private ApplicationDbContext _dbConnect = new ApplicationDbContext();
        // GET: Admin/ProductStock
        public ActionResult Index()
        {

            return View();
        }
        public ActionResult AddStock(int id)
        {
            Product product = _dbConnect.Product.Where(x => x.Product_Id == id).Single();
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.P_Id = product.Product_Id;
            ViewBag.ProductName = product.Name;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddStock([Bind(Include = "Id, Size, Quantity, Price, Product_Id")] ProductStock stock, string ProductName)
        {
            if (ModelState.IsValid)
            {
                _dbConnect.ProductStock.Add(stock);
                _dbConnect.SaveChanges();
                return RedirectToAction("Detail", "Product", new { id = stock.Product_Id });
            }
            ViewBag.P_Id = stock.Product_Id;
            ViewBag.ProductName = ProductName;
            return View(stock);
        }
        [HttpPost]
        public ActionResult Delete(int id)
        {
            var item = _dbConnect.ProductStock.Find(id);
            if (item != null)
            {
                _dbConnect.ProductStock.Remove(item);
                _dbConnect.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }

    }
}