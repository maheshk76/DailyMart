using DailyMart.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DailyMart.ViewModels
{
    public class WishlistViewModel
    {
        public int Id { get; set; }
        public Product Product { get; set; }
        public string UserId { get; set; }
    }
}