﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DailyMart.Models;
using DailyMart.ViewModels;

namespace DailyMart.Controllers
{
    [Authorize(Roles = "Admin")]

    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;
        public ProductsController()
        {
            _context = new ApplicationDbContext();
        }

        // GET: Products
        public ActionResult Index(string search)
        {
            var products = _context.Products.ToList();
            if (!string.IsNullOrEmpty(search))
            {
                products = products.Where(p => p.Name != null && p.Name.ToLower().Contains(search.ToLower())).ToList();
            }
            return View(products);
        }
        public ActionResult ProductTable(string search)
        {
            var products = _context.Products.ToList();
            if (!string.IsNullOrEmpty(search))
            {
                products = products.Where(p => p.Name != null && p.Name.ToLower().Contains(search.ToLower())).ToList();
            }
            return View(products);
        }

        // GET: Products/Details/5
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

        // GET: Products/Create
        public ActionResult Create()
        {

            NewProductViewModel model = new NewProductViewModel
            {
                AvailableCategories = _context.Category.ToList()
            };
            return View(model);
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]

        public ActionResult Create(NewProductViewModel model)
        {
            if (ModelState.IsValid)
            {
                var newProduct = new Product
                {
                    Name = model.Name,
                    Description = model.Description,
                    OriginalPrice = model.OriginalPrice,
                    SellingPrice = model.SellingPrice,
                    Stock = model.stock,
                    Tags = model.Tags,
                    Category = _context.Category.Find(model.CategoryID),

                    ImageURL = model.ImageURL
                };
                /* newProduct.CreatedOn = DateTime.Now;
                 newProduct.UpdatedOn = DateTime.Now;*/
                _context.Products.Add(newProduct);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(model);
        }

        // GET: Products/Edit/5
        public ActionResult Edit(int? id)
        {
            EditProductViewModel model = new EditProductViewModel();


            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = _context.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            model.ID = product.Id;
            model.Name = product.Name;
            model.Description = product.Description;
            model.OriginalPrice = product.OriginalPrice;
            model.SellingPrice = Convert.ToDecimal(product.SellingPrice);
            model.stock = product.Stock;
            model.CategoryID = product.Category != null ? product.Category.Id : 0;
            model.ImageURL = product.ImageURL;
            model.Tags = product.Tags;

            model.AvailableCategories = _context.Category.ToList();

            return View(model);

        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Edit(EditProductViewModel model)
        {
            if (ModelState.IsValid)
            {
                var existingProduct = _context.Products.Find(model.ID);
                existingProduct.Name = model.Name;
                existingProduct.Description = model.Description;
                existingProduct.OriginalPrice = model.OriginalPrice;
                existingProduct.SellingPrice = model.SellingPrice;
                existingProduct.Stock = model.stock;
                existingProduct.Category = null; //mark it null. Because the referncy key is changed below
                existingProduct.CategoryId = model.CategoryID;
                //existingProduct.Category = _context.Categories.Find(model.CategoryID);
                existingProduct.Tags = model.Tags;
               // existingProduct.UpdatedOn = DateTime.Now;
                //don't update imageURL if its empty
                if (!string.IsNullOrEmpty(model.ImageURL))
                {
                    existingProduct.ImageURL = model.ImageURL;
                }
                _context.Entry(existingProduct).State = EntityState.Modified;
                _context.SaveChanges();
                return RedirectToAction("Index");

            }


            return View(model);

        }

        // GET: Products/Delete/5
        public ActionResult Delete(int? id)
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

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = _context.Products.Find(id);
            _context.Products.Remove(product);
            _context.SaveChanges();
            return RedirectToAction("Index");
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