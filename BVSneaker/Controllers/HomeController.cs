using BVSneaker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BVSneaker.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext _dbConnect = new ApplicationDbContext();
        public ActionResult Index()
        {
            var items = _dbConnect.Advertisements.ToList();
            return View(items);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}