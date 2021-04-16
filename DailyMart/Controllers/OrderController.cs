using DailyMart;
using DailyMart.Models;
using DailyMart.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
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
    [Authorize(Roles = "Admin")]
    public class OrderController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private readonly ApplicationDbContext db = new ApplicationDbContext();
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

        // GET: Order

        public ActionResult Index(String status)
        {
            var orders = db.Orders.ToList();
            if (!string.IsNullOrEmpty(status))
            {
                orders = orders.Where(x => x.OrderStatus.ToLower().Contains(status.ToLower())).ToList();
            }
            return View(orders);
        }
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            string userId = db.Orders.Where(o => o.Id == id).Select(o => o.UserId).FirstOrDefault();
            OrderDetailViewModel model = new OrderDetailViewModel()
            {
                Order = db.Orders.Find(id),
                User = UserManager.FindById(userId),
                Address = db.Address.FirstOrDefault(o => o.UserId == userId)
            };
            if (model.Order == null)
            {
                return HttpNotFound();
            }
            if (model.Order.OrderStatus == "Cancelled")
            {
                ViewBag.AvailableStatuses = new List<string>() { model.Order.OrderStatus };
            }
            else
            {
                ViewBag.AvailableStatuses = new List<string>() { "Pending", "InProgress", "Delivered" };
            }
            return View(model);
        }
        public async Task<JsonResult> ChangeStatus(string status, int ID)
        {
            JsonResult result = new JsonResult
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };

            var order = db.Orders.Find(ID);

            order.OrderStatus = status;

            db.Entry(order).State = EntityState.Modified;


            result.Data = new { Success = db.SaveChanges() > 0 };
           
            var callbackUrl = Url.Action("MyOrders", "Home", null, protocol: Request.Url.Scheme);
            string body = "<html>Your order is " + status + "  <br/>Manage your orders here <a href=\"" + callbackUrl + "\">MyOrders</a></html>";
            await UserManager.SendEmailAsync(order.UserId, "Order Placed", body);
            return result;
        }
    }
}