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
    public class ProductImageController : Controller
    {
        private ApplicationDbContext _dbConnect = new ApplicationDbContext();
        // GET: Admin/ProductImage
        public ActionResult Index(int id)
        {
            ViewBag.ProductId = id;
            var items = _dbConnect.ProductImage.Where(x => x.Product_Id == id).ToList();
            return View(items);
        }
        public ActionResult AddImage(int productId, string url)
        {
            _dbConnect.ProductImage.Add(new ProductImage
            {
                Product_Id = productId,
                Image = url,
                isDefault = false
            });
            _dbConnect.SaveChanges();
            return Json(new { Success = true });
        }
        [HttpPost]
        public ActionResult Delete(int id)
        {
            var item = _dbConnect.ProductImage.Find(id);
            _dbConnect.ProductImage.Remove(item);
            _dbConnect.SaveChanges();
            return Json(new { success = true });
        }
    }
}