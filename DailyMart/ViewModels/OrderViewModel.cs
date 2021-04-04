using DailyMart.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DailyMart.ViewModels
{
    public class OrderViewModel
    {
        public List<Order> PendingOrders { get; set; }
        public List<Order> InProgressOrders { get; set; }
        public List<Order> PreviousOrders { get; set; }

        public List<Order> CancelledOrders { get; set; }
    }
}