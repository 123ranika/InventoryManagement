using InventoryManagement.Areas.Admin.Data;
using InventoryManagement.Data;
using InventoryManagement.DataModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace InventoryManagement.Areas.Admin.Controllers
{
    [Area("Admin"), Route("Invoice")]
    public class InvoiceController : Controller
    {
        private readonly ApplicationDbContext _context;

        public InvoiceController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Route("SaleCreate")]
        public IActionResult SaleCreate()
        {
            return View();
        }

        [HttpGet("SearchClientByPhone")]
        public IActionResult SearchClientByPhone(string phone)
        {
            if (string.IsNullOrEmpty(phone) || phone.Length < 4)
            {
                return Json(null);
            }
            try
            {
                var references = _context.Clients
                    .Where(x => x.Phone.StartsWith(phone))
                    .Select(x => new
                    {
                        x.ClientID,
                        x.ClientName,
                        x.Phone,
                        x.Address
                    })
                    .Take(5)
                    .ToList();

                return Json(new { success = true, references });
            }
            catch
            {
                return Json(new { success = false, message = "Error"});
            }
        }
        [HttpGet("SearchProductByName")]
        public IActionResult SearchProductByName(string description)
        {
            if (string.IsNullOrEmpty(description) || description.Length < 4)
            {
                return Json(null);
            }
            try
            {
                var references = _context.Products
                    .Where(x => x.ProductName.StartsWith(description))
                    .Select(x => new
                    {
                        x.ProductID,
                        x.ProductName,
                        x.Price,
                    })
                    .Take(5)
                    .ToList();

                return Json(new { success = true, references });
            }
            catch
            {
                return Json(new { success = false, message = "Error"});
            }
        }


        [HttpPost("OrderSummarySubmit")]
        public IActionResult OrderSummarySubmit([FromBody] InvoiceVM model)
        {
            if (model == null)
            {
                return Json(new { success = false, message = "Invalid invoice data." });
            }

            // You can log or store the model here

            return Json(new { success = true, message = "Invoice received successfully." });
        }


        [Route("SaleList")]
        public IActionResult SaleList()
        {
            var datalist = _context.Invoice.ToList();

            return View(datalist);
        }
        
    }
}

