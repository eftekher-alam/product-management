using Evidence6.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Evidence6.Models.Product;

namespace Evidence6.Controllers
{
    public class CategoriesController : Controller
    {
        readonly StoreDbContext db;
        public CategoriesController(StoreDbContext db) { this.db = db; }
        public IActionResult Index()
        {
            return View(db.Categories.ToList());
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Category c)
        {
            if (ModelState.IsValid)
            {
                db.Categories.Add(c);
                db.SaveChanges();
                return PartialView("_Result", true);
            }
            return PartialView("_Result", false);
        }
        public IActionResult Edit(int id)
        {
            return View(db.Categories.First(x => x.CategoryId == id));
        }
        [HttpPost]
        public IActionResult Edit(Category c)
        {
            if (ModelState.IsValid)
            {
                db.Entry(c).State = EntityState.Modified;
                db.SaveChanges();
                return PartialView("_Result", true);
            }
            return PartialView("_Result", false);
        }
        public IActionResult Delete(int id)
        {
            return View(db.Categories.First(x => x.CategoryId == id));
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DoDelete(int id)
        {
            var category = new Category { CategoryId = id };
            if (!db.Products.Any(x => x.CategoryId == id))
            {
                db.Entry(category).State = EntityState.Deleted;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", "Cannot delete. Company has related employees.");
            return View(db.Categories.First(x => x.CategoryId == id));


        }
    }
}
