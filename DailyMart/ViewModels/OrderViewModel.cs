using DailyMart.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DailyMart.ViewModels
{
    public class OrderViewModel
    {
       /* public List<Order> PendingOrders { get; set; }
        public List<Order> InProgressOrders { get; set; }
        public List<Order> PreviousOrders { get; set; }

        public List<Order> CancelledOrders { get; set; }*/
        public List<Order> MyOrders { get; internal set; }
    }

    public class OrderData
    {
        public decimal? Amount { get; set; }
        public string PaymentType { get; set; }

        public bool NewAddress { get; set; }

        public string AddressLine { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
    }
}