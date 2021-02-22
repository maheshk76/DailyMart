using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        [Required]
        public virtual IEnumerable<OrderItem> OrderItems { get; set; }

        public virtual Payment Payment { get; set; }
    }
}