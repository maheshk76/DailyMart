using System;
using System.Collections.Generic;

namespace DailyMart.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string Description { get; set; }
        public Nullable<DateTime> UpdateOn { get; set; }
        public Nullable<DateTime> CreatedOn { get; set; }
        public virtual List<Product> Products { get; set; }
    }
}