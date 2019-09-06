﻿using Microsoft.EntityFrameworkCore;
using Pavilion.DataAccess.Abstract;
using Pavilion.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Pavilion.DataAccess.Concrete.EfCore
{
    public class EfCoreProductDal : EfCoreGenericRepository<Product, ShopContext>, IProductDal
    {
        public int GetCountByCategory(string category)
        {
            using (var context = new ShopContext())  //defining the context as ShopContext
            {
                var products = context.Products.AsQueryable();

                if (!string.IsNullOrEmpty(category)) //filtering
                {
                    products = products //Reaching to the products and checking if it has any item in it
                         .Include(i => i.ProductCategories)
                         .ThenInclude(i => i.Category)
                         .Where(i => i.ProductCategories.Any(a => a.Category.Name.ToLower() == category.ToLower()));
                }
                //How many item will be skipped and taken each time the user changes pages
                return products.Count();
            }
        }

        public Product GetProductDetails(int id)
        {
            using (var context = new ShopContext())
            {
                return context.Products
                    .Where(i => i.Id == id)
                    .Include(i => i.ProductCategories)
                    .ThenInclude(i => i.Category)
                    .FirstOrDefault();
            }
        }

        public List<Product> GetProductsByCategory(string category, int page, int pageSize)
        {
            using (var context = new ShopContext())  //defining the context as ShopContext
            {
                var products = context.Products.AsQueryable();

                if (!string.IsNullOrEmpty(category)) //filtering
                {
                    products = products //Reaching to the products and checking if it has any item in it
                         .Include(i => i.ProductCategories)
                         .ThenInclude(i => i.Category)
                         .Where(i => i.ProductCategories.Any(a => a.Category.Name.ToLower() == category.ToLower()));
                }
                //How many item will be skipped and taken each time the user changes pages
                return products.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            }
        }
    }
}