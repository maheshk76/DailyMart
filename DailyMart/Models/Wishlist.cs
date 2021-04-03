using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DailyMart.Models
{
    public class Wishlist
    {
        [Key]
        public int Id { get; set; }
        public int ProductId {get;set;}

        public string UserId { get; set; }

    }
}