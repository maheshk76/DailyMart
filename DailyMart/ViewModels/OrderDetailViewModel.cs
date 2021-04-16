using DailyMart.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DailyMart.ViewModels
{
    public class OrderDetailViewModel
    {
        public Order Order { get; set; }
        public Address Address { get; set; }
        public ApplicationUser User { get; set; }
    }
}