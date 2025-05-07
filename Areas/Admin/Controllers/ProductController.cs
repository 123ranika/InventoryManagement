using InventoryManagement.Data;
using InventoryManagement.DataModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagement.Areas.Admin.Controllers
{
    [Area("Admin"), Route("Product")]
    //[Authorize]
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductController(ApplicationDbContext context)
        {
            _context = context;
        }


        [Route("ProductAdd")]
        public IActionResult ProductAdd()
        {
            var VendorList = _context.Vendors.ToList();
            var CategoryList = _context.Categorys.ToList();

            ViewBag.VendorList = VendorList;

            ViewBag.CategoryList = CategoryList;

            return View();
        }


        [HttpPost("ProductAddSubmit")]
        public IActionResult ProductAddSubmit(Products model)
        {

            if (model.ProductName != null)
            {
                if (model.Description == null)
                {
                    model.Description = "Add product description";
                }

                bool namecheck = _context.Products.Where(anika => anika.ProductName == model.ProductName).Any();

                if (!namecheck)
                {
                    _context.Products.Add(model);
                    _context.SaveChanges();
                }


                return RedirectToAction("ProductList");

            }

            return RedirectToAction("ProductAdd");
        }

        [Route("ProductEdit")]
        public IActionResult ProductEdit(Guid id)
        {
            var VendorList = _context.Vendors.ToList();
            var CategoryList = _context.Categorys.ToList();

            ViewBag.VendorList = VendorList;

            ViewBag.CategoryList = CategoryList;
            var data = _context.Products.Find(id);
            if (data == null)
            {
                return NotFound();
            }
            return View(data);
                
        }
        [HttpPost("ProductEdit")]
        public IActionResult ProductEdit(Products product)//int km = 10
        {
            //int km = 5
            var datacheck = _context.Products.Where(x => x.ProductID == product.ProductID).FirstOrDefault();

            if (datacheck != null)
            {
                datacheck.ProductName = product.ProductName;
                datacheck.Price = product.Price;
                datacheck.Quantity = product.Quantity;
                datacheck.Description = product.Description;
                datacheck.Category = product.Category;
                _context.Update(datacheck);
                _context.SaveChanges();
            }
            return RedirectToAction("ProductList");
        }
        [Route("ProductList")]
        public IActionResult ProductList()
        {
            var datalist = _context.Products.ToList();

            if (!datalist.Any())
            {
                return RedirectToAction("ProductList");
            }

            return View(datalist);
        }


        [Route("DeleteProduct")]
        public IActionResult DeleteProduct(Guid id)
        {
            var product = _context.Products.FirstOrDefault(c => c.ProductID == id);
            if (product != null)
            {
                _context.Products.Remove(product);
                _context.SaveChanges();
            }

            return RedirectToAction("ProductList");
        }
    }
}