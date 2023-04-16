using BVSneaker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BVSneaker.Controllers
{
    public class MenuController : Controller
    {
        private ApplicationDbContext _dbConnect = new ApplicationDbContext();
        // GET: Menu
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult MenuTop()
        {
            var items = _dbConnect.Menu.OrderBy(x => x.Position).ToList();
            return PartialView("_MenuTop", items);
        }
        public ActionResult AdvertisementSlider()
        {
            var items = _dbConnect.Advertisements.ToList();
            return PartialView("_AdvertisementSlider", items);
        }
        public ActionResult BrandSlider()
        {
            var items = _dbConnect.Brand.ToList();
            return PartialView("_BrandSlider", items);
        }
        public ActionResult MenuCategory()
        {
            var items = _dbConnect.Topic.ToList();
            return PartialView("_MenuTopic", items);
        }
        public ActionResult MenuArrivals()
        {
            var items = _dbConnect.Topic.ToList();
            return PartialView("_MenuArrivals", items);
        }
        public ActionResult MenuLeft(int? id)
        {
            if (id != null)
            {
                ViewBag.TopicId = id;
            }
            var items = _dbConnect.Topic.ToList();
            return PartialView("_MenuLeft", items);
        }
        public ActionResult MenuLeftBrand(int? id)
        {
            if (id != null)
            {
                ViewBag.BrandId = id;
            }
            var items = _dbConnect.Brand.ToList();
            return PartialView("_MenuLeftBrand", items);
        }

        public ActionResult MenuLeftCate(int? id)
        {
            if (id != null)
            {
                ViewBag.CategoryId = id;
            }
            var items = _dbConnect.Category.ToList();
            return PartialView("_MenuLeftCate", items);
        }
    }
}