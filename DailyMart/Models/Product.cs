using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DailyMart.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal OriginalPrice { get; set; }
        public decimal? SellingPrice { get; set; }
        public string ImageURL { get; set; }
        public string Tags { get; set; }
        public virtual Category Category { get; set; }
    }
}