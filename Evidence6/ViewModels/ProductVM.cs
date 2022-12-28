using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Evidence6.ViewModels
{
    public class ProductVM
    {
        public int ProductId { set; get; }
        [Required, StringLength(50), Display(Name = "Product Name")]
        public string ProductName { set; get; }
        [Required]
        public decimal Price { set; get; }
        [Required, Display(Name = "Expiry Date"), DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime ExpiryDate { set; get; }
        public IFormFile Picture { set; get; }
        [Required, ForeignKey("Category")]
        public int CategoryId { set; get; }
    }
}
