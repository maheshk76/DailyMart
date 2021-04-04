using DailyMart.Models;
using DailyMart.Services;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace DailyMart.Controllers
{
    [Authorize(Roles = "Admin")]

    public class CategoryController : Controller
    {
        public  string TimeAgo(DateTime dateTime)
        {
            string result = string.Empty;
            var timeSpan = DateTime.Now.Subtract(dateTime);

            if (timeSpan <= TimeSpan.FromSeconds(60))
            {
                result = string.Format("{0} seconds ago", timeSpan.Seconds);
            }
            else if (timeSpan <= TimeSpan.FromMinutes(60))
            {
                result = timeSpan.Minutes > 1 ?
                    String.Format("about {0} minutes ago", timeSpan.Minutes) :
                    "about a minute ago";
            }
            else if (timeSpan <= TimeSpan.FromHours(24))
            {
                result = timeSpan.Hours > 1 ?
                    String.Format("about {0} hours ago", timeSpan.Hours) :
                    "about an hour ago";
            }
            else if (timeSpan <= TimeSpan.FromDays(30))
            {
                result = timeSpan.Days > 1 ?
                    String.Format("about {0} days ago", timeSpan.Days) :
                    "yesterday";
            }
            else if (timeSpan <= TimeSpan.FromDays(365))
            {
                result = timeSpan.Days > 30 ?
                    String.Format("about {0} months ago", timeSpan.Days / 30) :
                    "about a month ago";
            }
            else
            {
                result = timeSpan.Days > 365 ?
                    String.Format("about {0} years ago", timeSpan.Days / 365) :
                    "about a year ago";
            }

            return result;
        }
        private readonly ApplicationDbContext _context;
        public CategoryController()
        {
            _context = new ApplicationDbContext();
        }
        public ActionResult Index(string search)
        {
            List<Category> categories = _context.Category.Include(x=>x.Products).ToList();
            if (!string.IsNullOrEmpty(search))
            {
                categories = categories.Where(c => c.Name != null && c.Name.ToLower().Contains(search.ToLower())).ToList();
            }
            ViewBag.LastUpdate = TimeAgo(Convert.ToDateTime(categories.OrderByDescending(c => c.UpdateOn).Select(c=>c.UpdateOn).FirstOrDefault()));
            return View(categories);

        }
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = _context.Category.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(Category category)
        {
            if (ModelState.IsValid)
            {
                if (category.Id == 0)
                {
                    category.CreatedOn = category.UpdateOn = DateTime.Now;
                    _context.Category.Add(category);
                }
                else
                {
                    category.UpdateOn = DateTime.Now;
                    _context.Entry(category).State = EntityState.Modified;
                }
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(category);
        }
        public ActionResult CategoryForm(int? id)
        {
            Category category = new Category();
            if (id == null)
                ViewBag.ActionType = "Create";
            else
            {
                ViewBag.ActionType = "Edit";
                category = _context.Category.Find(id);
                if (category == null)
                {
                    return HttpNotFound();
                }
            }
            return View(category);
        }
        [HttpPost]
        public JsonResult Delete(int id)
        {
            Category category = _context.Category.Find(id);
            _context.Category.Remove(category);
            _context.SaveChanges();
            JsonResult result = new JsonResult();
            result.Data=new { Success=true,Message="Category is deleted sucessfully"};
            return result;
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}