using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using DailyMart.Models;
using DailyMart.ViewModels;
using Microsoft.AspNet.Identity;

namespace DailyMart.Controllers
{
    public class SharedController : Controller
    {
        private readonly ApplicationDbContext ctx = new ApplicationDbContext();
        [Authorize(Roles = "Admin")]
        public JsonResult UploadImage()
        {
            JsonResult result = new JsonResult
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };

            try
            {
                var file = Request.Files[0];

                var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                var path = Path.Combine(Server.MapPath("~/content/products/images/"), fileName);

                file.SaveAs(path);

                result.Data = new { Success = true, ImageURL = string.Format("/content/products/images/{0}", fileName) };


            }
            catch (Exception ex)
            {
                result.Data = new { Success = false, ex.Message };
            }

            return result;
        }

        public JsonResult DecreaseQty(int productId)
        {
            JsonResult result = new JsonResult
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
            var q = 0;
            if (Session["cart"] != null)
            {
                List<Item> cart = (List<Item>)Session["cart"];
                var product = ctx.Products.Find(productId);

                foreach (var item in cart)
                {
                    if (item.Product.Id == productId)
                    {
                        int prevQty = item.Quantity;
                        if (prevQty > 0)
                        {
                            cart.Remove(item);
                            cart.Add(new Item()
                            {
                                Product = product,
                                Quantity = prevQty - 1
                            });
                            q = prevQty - 1;
                        }
                        break;
                    }
                }


                Session["cart"] = cart;
                var itemCount = cart.Count();
                result.Data = new { Success = true, Message = "Product Updated to cart successfully", CartLength = itemCount, Q = q };
            }

            return result;
        }
        public JsonResult IncreaseQty(int productId)
        {
            JsonResult result = new JsonResult
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
            var q = 0;
            if (Session["cart"] != null)
            {
                List<Item> cart = (List<Item>)Session["cart"];
                var product = ctx.Products.Find(productId);

                foreach (var item in cart)
                {
                    if (item.Product.Id == productId)
                    {
                        int prevQty = item.Quantity;
                        if (prevQty >= 0)
                        {
                            cart.Remove(item);
                            cart.Add(new Item()
                            {
                                Product = product,
                                Quantity = prevQty + 1
                            });
                            q = prevQty + 1;
                        }
                        break;
                    }
                }


                Session["cart"] = cart;
                var itemCount = cart.Count();
                result.Data = new { Success = true, Message = "Product Updated to cart successfully", CartLength = itemCount, Q = q };
            }

            return result;
        }
        public JsonResult AddToCart(int productId, int quantity)
        {
            Session["LastRemoved"] = null;
            JsonResult result = new JsonResult();
            var itemCount = 0;
            var q = 0;
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

            if (Session["cart"] == null)
            {
                List<Item> cart = new List<Item>();
                var product = ctx.Products.Find(productId);
                cart.Add(new Item()
                {
                    Product = product,
                    Quantity = quantity
                });
                q = quantity;
                Session["cart"] = cart;
                itemCount = cart.Count();
            }
            else
            {
                List<Item> cart = (List<Item>)Session["cart"];
                var count = cart.Count();
                if (count == 0)
                {
                    cart = new List<Item>();
                    var product1 = ctx.Products.Find(productId);
                    cart.Add(new Item()
                    {
                        Product = product1,
                        Quantity = quantity
                    });
                    q = quantity;
                    Session["cart"] = cart;
                    itemCount = cart.Count();
                }
                else
                {
                    var product = ctx.Products.Find(productId);

                    for (int i = 0; i < count; i++)
                    {
                        if (cart[i].Product.Id == productId)
                        {
                            int prevQty = cart[i].Quantity;
                            cart.Remove(cart[i]);

                            cart.Add(new Item()
                            {
                                Product = product,
                                Quantity = quantity
                            });
                            break;
                        }
                        else
                        {
                            var prd = cart.Where(x => x.Product.Id == productId).SingleOrDefault();
                            if (prd == null)
                            {
                                cart.Add(new Item()
                                {
                                    Product = product,
                                    Quantity = quantity
                                });
                            }
                        }
                    }
                    Session["cart"] = cart;
                    itemCount = cart.Count();
                }


            }
            result.Data = new { Success = true, Message = "Product Added to cart successfully", CartLength = itemCount };
            return result;
        }
        public JsonResult RemoveFromCart(int productId)
        {
            JsonResult result = new JsonResult
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };

            List<Item> cart = (List<Item>)Session["cart"];
            foreach (var item in cart)
            {
                if (item.Product.Id == productId)
                {
                    cart.Remove(item);
                    Session["LastRemoved"] = item;
                    break;
                }
            }
            
            Session["cart"] = cart;
            var itemCount = cart.Count();
            result.Data = new { Success = true, Message = "Product Removed from cart successfully", CartLength = itemCount };
            return result;
        }
        public JsonResult MakeWishlist(int productId, int act)
        {
            string userId = User.Identity.GetUserId();
            JsonResult result = new JsonResult
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
            if (act == 1)
            {
                Wishlist wishlist = new Wishlist()
                {
                    UserId = User.Identity.GetUserId(),
                    ProductId = productId

                };
                ctx.Wishlists.Add(wishlist);
                ctx.SaveChanges();
                result.Data = new { Success = true, Message = "Product Added to your Wishlist" };
                return result;
            }
            else
            {

                var wish = ctx.Wishlists.FirstOrDefault(w => w.ProductId == productId && w.UserId == userId);
                ctx.Wishlists.Remove(wish);
                ctx.SaveChanges();
                result.Data = new { Success = true, Message = "Product Removed from Wishlist" };
                return result;
            }
        }
        public ActionResult Error()
        {
            return View();
        }
    }


}