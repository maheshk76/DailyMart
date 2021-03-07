using DailyMart.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DailyMart.ViewModels
{
    public class ProductSearchViewModel
    {
        public List<Product> Products { get; set; }
        public string SearchTerm { get; set; }

       
    }
    public class NewProductViewModel
    {
        
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal OriginalPrice { get; set; }
        public decimal SellingPrice { get; set; }
        public int stock { get; set; }
        [Display(Name = "Image")]
        public string ImageURL { get; set; }
        public string Tags { get; set; }
        public int CategoryID { get; set; }
        public List<Category> AvailableCategories { get; set; }
    }

    public class EditProductViewModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal OriginalPrice { get; set; }
        public decimal SellingPrice { get; set; }
        public int stock { get; set; }
        [Display(Name = "Image")]
        public string ImageURL { get; set; }
        public string Tags { get; set; }
        public int CategoryID { get; set; }
        public List<Category> AvailableCategories { get; set; }
    }
    public class ShopViewModel
    {
        public int MaximumPrice { get; set; }
        public int MinimumPrice { get; set; }
        public int? SortBy { get; set; }
        public int? CategoryID { get; set; }
        public string Search{ get; set; }
    }

    public class ProductViewModel
    {
        public Product Product { get; set; }
    }
}