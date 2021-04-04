using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DailyMart.Models
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public string OrderStatus { get; set; }
        [Required]
        public decimal? Amount { get; set; }

        [Required]
        public string UserId { get; set; }

        public virtual List<OrderItem> OrderItems { get; set; }
        public string PaymentType { get; set; }
    }
}