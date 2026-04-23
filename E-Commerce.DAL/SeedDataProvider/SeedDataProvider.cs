using E_Commerce.DAL.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.DAL
{
    public static class SeedDataProvider
    {
        public static List<Category> GetCategories()
        {
            var createdDate = new DateTime(2026, 3, 1);

            // ===== Categories Seed =====
            return new List<Category>() 
            {
                new Category { Name = "Electronics", CreatedAt = createdDate },
                new Category { Name = "Books", CreatedAt = createdDate },
                new Category { Name = "Clothes", CreatedAt = createdDate },
            };

        }

        public static List<Product> GetProducts()
        {
            var createdDate = new DateTime(2026, 1, 1);

            // ===== Categories Seed =====
            return new List<Product>()
            {
                new Product
                {
                    Title = "Laptop",
                    Description = "Core i7, 16GB RAM, 512GB SSD",
                    Price = 25000,
                    Count = 5,
                    CreatedAt = createdDate,
                    CategoryId = 1
                },
                new Product
                {
                    Title = "Headphones",
                    Description = "Wireless Bluetooth Headphones",
                    Price = 1500,
                    Count = 20,
                    CreatedAt = createdDate,
                    CategoryId = 1
                },
                new Product
                {
                    Title = "C# Programming Book",
                    Description = "Learn ASP.NET Core step by step",
                    Price = 500,
                    Count = 15,
                    CreatedAt = createdDate,
                    CategoryId = 2
                },
                new Product
                {
                    Title = "Novel Book",
                    Description = "A thrilling mystery novel",
                    Price = 200,
                    Count = 10,
                    CreatedAt = createdDate,
                    CategoryId = 2
                },
                new Product
                {
                    Title = "Jeans",
                    Description = "Slim fit denim jeans",
                    Price = 1200,
                    Count = 25,
                    CreatedAt = createdDate,
                    CategoryId = 3
                },
                new Product
                {
                    Title = "T-Shirt",
                    Description = "100% Cotton",
                    Price = 300,
                    Count = 30,
                    CreatedAt = createdDate,
                    CategoryId = 3
                }
            };

        }
    }
}
