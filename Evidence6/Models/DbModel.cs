using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Evidence6.Models
{
    public class Category
    {
        public Category() { this.Products = new List<Product>(); }
        public int CategoryId { set; get; }

        [Required, StringLength(50), Display(Name = "Category Name")]
        public string CategoryName { set; get; }
        [Required, StringLength(500)]
        public string Discription { set; get; }
        public virtual ICollection<Product> Products { set; get; }
    }

    public class Product
    {
        public int ProductId { set; get; }
        [Required, StringLength(50), Display(Name = "Product Name")]
        public string ProductName { set; get; }
        [Required, Column(TypeName = "money")]
        public decimal Price { set; get; }
        [Required, Column(TypeName = "date"), Display(Name = "Expiry Date"), DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]

        public DateTime ExpiryDate { set; get; }
        public string Picture { set; get; }
        [Required, ForeignKey("Category")]
        public int CategoryId { set; get; }
        public virtual Category Category { set; get; }
    }

        public class StoreDbContext : DbContext
        {
            public StoreDbContext(DbContextOptions<StoreDbContext> options) : base(options) { }
            public DbSet<Category> Categories { get; set; }
            public DbSet<Product> Products { get; set; }
        }
    
}
