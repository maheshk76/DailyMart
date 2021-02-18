using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DailyMart.Models
{
    public class Payment
    {
        public int Id { get; set; }
        [Required]
        public string Status { get; set; }
        [Required]
        public string Type { get; set; }
    }
}