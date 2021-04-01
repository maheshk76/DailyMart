
using DailyMart.Models;
using DailyMart.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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
        [Authorize]
        public ActionResult CheckOut()
        {
            ApplicationUser user = UserManager.FindById(User.Identity.GetUserId());
            ViewBag.User = user;
            return View();
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
        public JsonResult PlaceOrder(Order model)
        {
            JsonResult result = new JsonResult();
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            Order newOrder = new Order();
            newOrder.UserId = User.Identity.GetUserId();
            newOrder.OrderDate = DateTime.Now;
            newOrder.OrderStatus = "Pending";
            /*newOrder.FirstName = model.FirstName;
            newOrder.LastName = model.LastName;
            newOrder.PhoneNumber = model.PhoneNumber;
            newOrder.Address1 = model.Address1;
            newOrder.Address2 = model.Address2;
            newOrder.City = model.City;
            newOrder.ZipCode = model.ZipCode;*/
            newOrder.Amount = model.Amount;
            //newOrder.PaymentType = model.PaymentType;
            if (Session["cart"] != null)
            {
                List<Item> cart = (List<Item>)Session["cart"];
                List<OrderItem> orderitems = new List<OrderItem>();
                foreach (var item in cart)
                {

                    if (item.Quantity > 0)
                    {
                        OrderItem orderItem = new OrderItem();
                        orderItem.Quantity = item.Quantity;
                        orderItem.ProductId = item.Product.Id;
                        Product p = _context.Products.Find(item.Product.Id);
                        p.Stock = p.Stock - item.Quantity;
                        orderitems.Add(orderItem);

                    }
                }

                newOrder.OrderItems = orderitems;

                _context.Orders.Add(newOrder);
                _context.SaveChanges();
                Session["cart"] = null;
                result.Data = new { Success = true, Message = "Product Updated to cart successfully" };

            }

            return result;
        }
        [Authorize]
        public ActionResult MyOrders(string userId, string status)
        {
            var orders = _context.Orders.Where(x => x.Id == Convert.ToInt32(userId)).Include(x => x.OrderItems);
            OrderViewModel model = new OrderViewModel();
            model.PendingOrders = orders.Where(x => x.OrderStatus.ToLower().Contains("pending")).OrderByDescending(x => x.OrderDate).ToList();
            model.InProgressOrders = orders.Where(x => x.OrderStatus.ToLower().Contains("inprogress")).OrderByDescending(x => x.OrderDate).ToList();
            model.PreviousOrders = orders.Where(x => x.OrderStatus.ToLower().Contains("delivered")).OrderByDescending(x => x.OrderDate).ToList();

            return View(model);
        }
    }
}