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
    public class MessageController : Controller
    {
        private readonly ApplicationDbContext _context = new ApplicationDbContext();

        // GET: Messages
        [Authorize(Roles = "Admin")]

        public ActionResult Index()
        {
            return View(_context.Messages.OrderByDescending(x => x.CreatedOn).ToList());
        }
        public int GetMessages()
        {
            return _context.Messages.Where(m=>m.isRead==false).Count();
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


        public ActionResult Send()
        {
            return View();
        }
        // POST: Messages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Send(Message message)
        {
            if (ModelState.IsValid)
            {
                message.isRead = false;
                message.CreatedOn = DateTime.Now;

                _context.Messages.Add(message);
                _context.SaveChanges();

            }
            if (User.IsInRole("Admin"))
                return RedirectToAction("Index");
            else
                return RedirectToAction("Index", "Home");
        }




        public JsonResult Delete(int ID)
        {
            JsonResult result = new JsonResult
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data = new { Success = false }
            };
            Message message = _context.Messages.Find(ID);
            _context.Messages.Remove(message);
            _context.SaveChanges();
            result.Data = new { Success = true ,Message= "Message deleted Successfully" };
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
