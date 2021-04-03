using DailyMart.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DailyMart.Controllers
{
    [Authorize(Roles = "Admin")]

    public class AdminController : Controller
    {

        private readonly ApplicationDbContext db;
        public AdminController()
        {
            db = new ApplicationDbContext();
        }
        public ActionResult DashBoard()
        {
            ViewBag.OrderCount = db.Orders.Count();
            ViewBag.ProductCount = db.Products.Count();
            ViewBag.CategoryCount = db.Category.Count();
            ViewBag.MessageCount = db.Messages.Count();
            return View();
        }
    }
}