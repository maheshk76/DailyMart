using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DailyMart.Models;

namespace DailyMart.Controllers
{
    public class MessagesController : Controller
    {
        private ApplicationDbContext _context = new ApplicationDbContext();

        // GET: Messages
        [Authorize(Roles = "Admin")]

        public ActionResult Index()
        {
            return View(_context.Messages.OrderByDescending(x => x.CreatedOn).ToList());
        }

        // GET: Messages/Details/5
        [Authorize(Roles = "Admin")]

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Message message = _context.Messages.Find(id);
            if (message == null)
            {
                return HttpNotFound();
            }
            message.isRead = true;
            _context.Entry(message).State = EntityState.Modified;
            _context.SaveChanges();
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

                _context.Messages.Add(message);
                _context.SaveChanges();
                result.Data = new { Success = true };

            }

            return result;
        }




        public JsonResult Delete(int ID)
        {
            JsonResult result = new JsonResult();
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            result.Data = new { Success = false };

            Message message = _context.Messages.Find(ID);
            _context.Messages.Remove(message);
            _context.SaveChanges();
            result.Data = new { Success = true };



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
