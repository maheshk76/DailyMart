using DailyMart.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace DailyMart.Services
{
    public class CategoryService
    {
        public Category GetCategory(int ID)
        {
            using (var context = new ApplicationDbContext())
            {
                return context.Category.Find(ID);
            }
        }


        public int GetCategoriesCount(string search)
        {
            using (var context = new ApplicationDbContext())
            {
                if (!string.IsNullOrEmpty(search))
                {
                    return context.Category.Where(category => category.Name != null &&
                         category.Name.ToLower().Contains(search.ToLower())).Count();
                }
                else
                {
                    return context.Category.Count();
                }
            }
        }

        public List<Category> GetAllCategories()
        {
            using (var context = new ApplicationDbContext())
            {
                return context.Category
                        .ToList();
            }
        }

        public List<Category> GetCategories(string search, int pageNo)
        {
            int pageSize = 3;

            using (var context = new ApplicationDbContext())
            {
                if (!string.IsNullOrEmpty(search))
                {
                    return context.Category.Where(category => category.Name != null &&
                         category.Name.ToLower().Contains(search.ToLower()))
                         .OrderBy(x => x.Id)
                         .Skip((pageNo - 1) * pageSize)
                         .Take(pageSize)
                         .Include(x => x.Products)
                         .ToList();
                }
                else
                {
                    return context.Category
                        .OrderBy(x => x.Id)
                        .Skip((pageNo - 1) * pageSize)
                        .Take(pageSize)
                        .Include(x => x.Products)
                        .ToList();
                }
            }
        }



        public void SaveCategory(Category category)
        {
            using (var context = new ApplicationDbContext())
            {
                context.Category.Add(category);
                context.SaveChanges();
            }
        }

        public void UpdateCategory(Category category)
        {
            using (var context = new ApplicationDbContext())
            {
                context.Entry(category).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();
            }
        }

        public void DeleteCategory(int ID)
        {
            using (var context = new ApplicationDbContext())
            {
                var category = context.Category.Where(x => x.Id == ID).Include(x => x.Products).FirstOrDefault();

                context.Products.RemoveRange(category.Products); //first delete products of this category
                context.Category.Remove(category);
                context.SaveChanges();
            }
        }
    }
}