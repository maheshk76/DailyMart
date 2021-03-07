using DailyMart.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DailyMart.ViewModels
{
    public class Item
    {
        public Product Product { get; set; }
        public int Quantity { get; set; }
    }
}