using DailyMart.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DailyMart.Services;
namespace DailyMart.Controllers
{
    [Authorize(Roles = "Admin")]

    public class CategoryController : Controller
    {
        
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
            ViewBag.LastUpdate =Convert.ToDateTime(categories.OrderByDescending(c => c.UpdateOn).Select(c=>c.UpdateOn).FirstOrDefault()).TimeAgo();
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
            JsonResult result = new JsonResult
            {
                Data = new { Success = true, Message = "Category is deleted sucessfully" }
            };
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