
using DailyMart.Models;
using DailyMart.ViewModels;
using Microsoft.AspNet.Identity.Owin;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace DailyMart.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private ApplicationDbContext _context;
        public HomeController()
        {
            _context = new ApplicationDbContext();
        }

        public HomeController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
            _context = new ApplicationDbContext();
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        public ActionResult Index(string search, int? reset, int? categoryID = null, int? sortBy = 0, int? minimumPrice = 0, int? maximumPrice = null, int? page = null)
        {
            if (TempData["products"] != null)
            {

                ViewBag.products = TempData["products"];
            }

            else
            {
                ViewBag.products = _context.Products.ToList();
            }
            ViewBag.Categories = _context.Category.ToList();
            ViewBag.MinimumPrice = 0;
            ViewBag.MaximumPrice = (int)(_context.Products.Max(x => x.SellingPrice));

            if (reset == 1)
            {

                ShopViewModel model1 = new ShopViewModel();
                model1.Search = search;
                model1.CategoryID = categoryID;
                model1.MinimumPrice = ViewBag.MinimumPrice;
                model1.MaximumPrice = (int)(_context.Products.Max(x => x.SellingPrice));
                model1.SortBy = sortBy;
                ViewBag.products = _context.Products.ToList();
                return View(model1);
            }


            if (minimumPrice.HasValue)
            {
                ViewBag.MinimumPrice = minimumPrice;
            }
            if (maximumPrice.HasValue)
            {
                ViewBag.MaximumPrice = maximumPrice;
            }
            ShopViewModel model = new ShopViewModel();

            if (TempData["search"] != null)
            {
                model.Search = (string)TempData["search"];
            }
            else
            {
                model.Search = search;
            }

            if (TempData["categoryID"] != null)
            {
                model.CategoryID = (int)TempData["categoryID"];
            }
            else
            {
                model.CategoryID = categoryID;
            }

            model.MaximumPrice = (int)(_context.Products.Max(x => x.SellingPrice));

            if (TempData["MinimumPrice"] != null)
            {
                model.MinimumPrice = (int)TempData["MinimumPrice"];
            }
            else
            {
                model.MinimumPrice = ViewBag.MinimumPrice;
            }

            if (TempData["search"] != null)
            {
                model.SortBy = (int)TempData["SortBy"];
            }
            else
            {
                model.SortBy = sortBy;
            }

            return View(model);
        }
        public ActionResult Products(string search, int? categoryID, int? page, int? minimumPrice, int? maximumPrice, int? sortBy, List<Product> products = null)
        {

            ViewBag.Search = search;
            ViewBag.CategoryID = categoryID;
            ViewBag.SortBy = sortBy;
            if (products == null)
            {
                products = _context.Products.ToList();
            }





            if (!string.IsNullOrEmpty(search))
            {
                products = products.Where(p => p.Category.Name.ToLower().Contains(search.ToLower()) || p.Name.ToLower().Contains(search.ToLower())).ToList();
            }
            else
            {
                if (categoryID.HasValue)
                {
                    products = products.Where(x => x.Category.Id == categoryID.Value).ToList();
                }

                if (minimumPrice.HasValue)
                {
                    products = products.Where(x => x.SellingPrice >= minimumPrice.Value).ToList();
                }

                if (maximumPrice.HasValue)
                {
                    products = products.Where(x => x.SellingPrice <= maximumPrice.Value).ToList();
                }

                if (sortBy.HasValue)
                {
                    switch (sortBy.Value)
                    {
                        case 2:
                            products = products.OrderByDescending(x => x.Id).ToList();
                            break;
                        case 3:
                            products = products.OrderBy(x => x.SellingPrice).ToList();
                            break;
                        case 4:
                            products = products.OrderByDescending(x => x.SellingPrice).ToList();
                            break;
                        default:
                            break;
                    }
                }
            }
            TempData["products"] = products;
            TempData["search"] = search;
            TempData["SortBy"] = sortBy;
            TempData["MinimumPrice"] = minimumPrice;

            TempData["categoryID"] = categoryID;
            IPagedList<Product> pr = products.ToPagedList(page ?? 1, 12);


            return PartialView(pr);
        }
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = _context.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
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
        public ActionResult Cart()
        {
            return View();
        }
    }
}