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

        private readonly ApplicationDbContext _context;
        public AdminController()
        {
            _context = new ApplicationDbContext();
        }
        public ActionResult DashBoard()
        {
            ViewBag.OrderCount = _context.Orders.Count();
            ViewBag.ProductCount = _context.Products.Count();
            ViewBag.CategoryCount = _context.Category.Count();
            ViewBag.MessageCount = _context.Messages.Count();
            return View();
        }
    }
}