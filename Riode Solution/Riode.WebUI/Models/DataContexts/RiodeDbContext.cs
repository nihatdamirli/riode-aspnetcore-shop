using Microsoft.EntityFrameworkCore;
using Riode.WebUI.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Riode.WebUI.Models.DataContexts
{
    public class RiodeDbContext : DbContext
    {
        public RiodeDbContext()
            : base()
        {

        }
        public RiodeDbContext(DbContextOptions options)
           : base(options)
        {

        }

        public DbSet<Contactpost> ContactPosts { get; set; }
        public DbSet<Faq> Faqs { get; set; }
        public DbSet<Subscribe> Subscribes { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<ProductColor> Colors { get; set; }
        public DbSet<ProductSize> Sizes { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<ProductSizeColorCollection> ProductSizeColorItem{ get; set; }
        public DbSet<Specifications> Specification{ get; set; }
        public DbSet<SpecificationsCategoryItem> SpecificationsCategoryCollection{ get; set; }
        public DbSet<SpecificationValue> SpecificationValues { get; set; }
        
    }
}
