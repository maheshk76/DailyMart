using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DailyMart.Models
{
    public class Message
    {
        public int ID { get; set; }
        [Required]
        [StringLength(10, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
        [Display(Name = " Name")]
        public string Name { get; set; }
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Required]
        [StringLength(30, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
        [Display(Name = " Subject")]
        public string Subject { get; set; }
        [Required]
        [StringLength(300, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
        [Display(Name = " Message")]
        public string Text { get; set; }
        [Display(Name = "Time")]
        public DateTime CreatedOn { get; set; }
        public bool isRead { get; set; }
    }
}