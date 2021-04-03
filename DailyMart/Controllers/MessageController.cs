using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DailyMart.Models;

namespace GroceryShop.Controllers
{
    public class MessagesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Messages
        [Authorize(Roles = "Admin")]

        public ActionResult Index()
        {
            return View(db.Messages.OrderByDescending(x => x.CreatedOn).ToList());
        }

        // GET: Messages/Details/5
        [Authorize(Roles = "Admin")]

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Message message = db.Messages.Find(id);
            if (message == null)
            {
                return HttpNotFound();
            }
            message.isRead = true;
            db.Entry(message).State = EntityState.Modified;
            db.SaveChanges();
            return View(message);
        }


        public ActionResult CreateForm()
        {
            return PartialView();
        }

        // POST: Messages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Create(Message message)
        {
            JsonResult result = new JsonResult();
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            result.Data = new { Success = false };
            if (ModelState.IsValid)
            {
                message.isRead = false;
                message.CreatedOn = DateTime.Now;

                db.Messages.Add(message);
                db.SaveChanges();
                result.Data = new { Success = true };

            }

            return result;
        }




        public JsonResult Delete(int ID)
        {
            JsonResult result = new JsonResult();
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            result.Data = new { Success = false };

            Message message = db.Messages.Find(ID);
            db.Messages.Remove(message);
            db.SaveChanges();
            result.Data = new { Success = true };



            return result;
        }





        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
