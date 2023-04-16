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
    public class OrderController : Controller
    {

            private ApplicationDbContext _dbConnect = new ApplicationDbContext();
        // GET: Admin/Order
        public ActionResult Index(int? page)
        {
            var items = _dbConnect.Order.OrderByDescending(x => x.CreateDate).ToList();
            if (page == null)
            {
                page = 1;
            }
            var pageNumber = page ?? 1;
            var pageSize = 10;
            ViewBag.PageSize = pageSize;
            ViewBag.Page = pageNumber;
            return View(items.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult OrderDetail(int id)
        {
            var item = _dbConnect.Order.Find(id);
                return View(item);
        }

        public ActionResult Partial_Product(int id)
        {
            var item = _dbConnect.OrderDetail.Where(x => x.Order_Id == id).ToList();
            return PartialView(item);
        }


        [HttpPost]
        public ActionResult UpdateStatus(int id, bool status)
        {
            var item = _dbConnect.Order.Find(id);
            if (item!=null)
            {
                _dbConnect.Order.Attach(item);
                item.IsPaid = status;
                _dbConnect.Entry(item).Property(x => x.PaymentMethod).IsModified = true;
                _dbConnect.SaveChanges();
                return Json(new { message="Success", Success = true });
            }
            return Json(new { message = "Unsuccess", Success = false });
        }
    }
}