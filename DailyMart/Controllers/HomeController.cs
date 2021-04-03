
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
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace DailyMart.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private readonly ApplicationDbContext _context;
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

                ShopViewModel model1 = new ShopViewModel
                {
                    Search = search,
                    CategoryID = categoryID,
                    MinimumPrice = ViewBag.MinimumPrice,
                    MaximumPrice = (int)(_context.Products.Max(x => x.SellingPrice)),
                    SortBy = sortBy
                };
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
            
                if (Request.IsAuthenticated)
                {
                    var user = User.Identity.GetUserId();
                    var wish = _context.Wishlists.Where(w => w.UserId == user).Select(w=>w.ProductId).ToList();
                if (wish.Count() == 0)
                    ViewBag.Wishlist = null;
                else
                    ViewBag.Wishlist = wish;
                }
            return PartialView(pr);
        }
        [Authorize]
        public ActionResult CheckOut()
        {
            ApplicationUser user = UserManager.FindById(User.Identity.GetUserId());
            ViewBag.User = user;
            var address=_context.Address.FirstOrDefault(a => a.UserId == user.Id);
            ViewBag.uAddress = address;
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
            var UserId = User.Identity.GetUserId();
           var InWish= _context.Wishlists.FirstOrDefault(w => w.ProductId == id && w.UserId == UserId);
            ViewBag.InWish = InWish;
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
        public ActionResult WishList()
        {
            List<WishlistViewModel> model = new List<WishlistViewModel>();
            var userId = User.Identity.GetUserId();
            var Wishlist = _context.Wishlists.Where(w => w.UserId == userId).ToList();
            List<Product> plist = new List<Product>();
            foreach(var wlist in Wishlist)
            {
                Product product=_context.Products.FirstOrDefault(p => p.Id == wlist.ProductId);
                model.Add(
                    new WishlistViewModel()
                    {
                        Id = wlist.Id,
                        UserId = userId,
                        Product = product
                    });

            }
           
            return View(model);
        }
        public async Task<JsonResult> PlaceOrder(string orderdata)
        {
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            
            var data = orderdata.Split('&');
            for(int i = 0; i < data.Length; i++)
            {
                data[i] = data[i].Split('=')[1];
            }
            JsonResult result = new JsonResult
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
            Order newOrder = new Order
            {
                UserId = User.Identity.GetUserId(),
                OrderDate = DateTime.Now,
                OrderStatus = "Pending",
                Amount = Convert.ToInt32(data[0])
            };
            if (data[1] == "true")
            {
                Address address = new Address
                {
                    UserId = User.Identity.GetUserId(),
                    AddressLine = data[2],
                    City = data[3],
                    State = data[4],
                    ZipCode = data[5]
                };
                _context.Address.Add(address);
                _context.SaveChanges();
                newOrder.PaymentType = data[6];
            }
            else
            {
                newOrder.PaymentType = data[2];
            }
            if (Session["cart"] != null)
            {
                List<Item> cart = (List<Item>)Session["cart"];
                List<OrderItem> orderitems = new List<OrderItem>();
                foreach (var item in cart)
                {

                    if (item.Quantity > 0)
                    {
                        OrderItem orderItem = new OrderItem
                        {
                            Quantity = item.Quantity,
                            ProductId = item.Product.Id
                        };
                        Product p = _context.Products.Find(item.Product.Id);
                        p.Stock -= item.Quantity;
                        orderitems.Add(orderItem);

                    }
                }

                newOrder.OrderItems = orderitems;

                _context.Orders.Add(newOrder);
                _context.SaveChanges();
                Session["cart"] = null;
                result.Data = new { Success = true, Message = "Product Updated to cart successfully" };

            }
            var callbackUrl = Url.Action("MyOrders", "Home", new { userId = user.Id}, protocol: Request.Url.Scheme);
            string body = "<html>Your order has been placed with Order Id:" + newOrder.Id+ "  <br/>Manage your orders here <a href=\"" + callbackUrl + "\">MyOrders</a></html>";
            await UserManager.SendEmailAsync(user.Id,"Order Placed",body);
            return result;
        }
        [Authorize]
        public ActionResult MyOrders(string userId, string status)
        {
            var orders = _context.Orders.Where(x => x.UserId == userId).Include(x => x.OrderItems).ToList();
            OrderViewModel model = new OrderViewModel
            {
                PendingOrders = orders.Where(x => x.OrderStatus.ToLower().Contains("pending")).OrderByDescending(x => x.OrderDate).ToList(),
                InProgressOrders = orders.Where(x => x.OrderStatus.ToLower().Contains("inprogress")).OrderByDescending(x => x.OrderDate).ToList(),
                PreviousOrders = orders.Where(x => x.OrderStatus.ToLower().Contains("delivered")).OrderByDescending(x => x.OrderDate).ToList()
            };

            return View(model);
        }
    }
}