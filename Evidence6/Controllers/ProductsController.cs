using Evidence6.Models;
using Evidence6.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;

namespace Evidence6.Controllers
{
    public class ProductsController : Controller
    {
        private readonly StoreDbContext db;
        private readonly IWebHostEnvironment env;
        public ProductsController(StoreDbContext db, IWebHostEnvironment env) 
        { 
            this.db = db; 
            this.env = env; 
        }

        public IActionResult Index()
        {
            return View(db.Products.Include(x => x.Category).ToList());
        }
        public IActionResult Create()
        {
            ViewBag.Categories = db.Categories.ToList();
            return View();
        }
        [HttpPost]
        public IActionResult Create(ProductVM p)
        {
            if (ModelState.IsValid)
            {
                var pNew = new Product
                {
                    ProductName = p.ProductName,
                    Price = p.Price,
                    ExpiryDate = p.ExpiryDate,
                    CategoryId = p.CategoryId,
                    Picture = "no-pic.png"
                };
                if (p.Picture != null && p.Picture.Length > 0)
                {
                    string dir = Path.Combine(env.WebRootPath, "Uploads");
                    if (!Directory.Exists(dir))
                    {
                        Directory.CreateDirectory(dir);
                    }
                    string fileName = Guid.NewGuid() + Path.GetExtension(p.Picture.FileName);
                    string fullPath = Path.Combine(dir, fileName);
                    FileStream fs = new FileStream(fullPath, FileMode.Create);
                    p.Picture.CopyTo(fs);
                    fs.Flush();
                    fs.Close();
                    pNew.Picture = fileName;
                }
                db.Products.Add(pNew);
                db.SaveChanges();
                return RedirectToAction("Index");

            }
            ViewBag.Categories = db.Categories.ToList();
            return View(p);
        }
        public IActionResult Edit(int Id)
        {
            ViewBag.Categories = db.Categories.ToList();

            var p = db.Products.Include(p => p.Category).First(e => e.ProductId == Id);
            ViewBag.CurrentPicture = p.Picture;

            return View(new ProductVM
            {
                ProductId = p.ProductId,
                ProductName = p.ProductName,
                Price = p.Price,
                ExpiryDate = p.ExpiryDate,
                CategoryId = p.CategoryId
            });
        }
        [HttpPost]
        public IActionResult Edit(ProductVM product)
        {
            var p = db.Products.First(e => e.ProductId == product.ProductId);
            if (ModelState.IsValid)
            {
                p.ProductName = product.ProductName;
                p.Price = product.Price;
                p.ExpiryDate = product.ExpiryDate;
                p.CategoryId = product.CategoryId;
                if (product.Picture != null && product.Picture.Length > 0)
                {
                    string dir = Path.Combine(env.WebRootPath, "Uploads");
                    if (!Directory.Exists(dir))
                    {
                        Directory.CreateDirectory(dir);
                    }
                    string fileName = Guid.NewGuid() + Path.GetExtension(product.Picture.FileName);
                    string fullPath = Path.Combine(dir, fileName);
                    FileStream fs = new FileStream(fullPath, FileMode.Create);
                    product.Picture.CopyTo(fs);
                    fs.Flush();
                    p.Picture = fileName;
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Brands = db.Categories.ToList();
            ViewBag.currentPicture = p.Picture;
            return View(product);
        }
        public IActionResult Delete(int Id)
        {
            return View(db.Products.Include(e => e.Category).First(e => e.ProductId == Id));
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DoDelete(int Id)
        {
            var Product = new Product { ProductId = Id };
            db.Entry(Product).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
