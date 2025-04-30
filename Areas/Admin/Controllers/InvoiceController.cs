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

            var clients = _context.Clients
                .Where(c => c.Phone.StartsWith(phone))
                .Select(c => new
                {
                    clientName = c.ClientName,
                    clientAddress = c.Address,
                    phone = c.Phone
                })
                .ToList(); // Important: ToList to return multiple results

            return Json(clients);
        }

        [HttpGet("/Invoice/SearchProductByName")]
        public IActionResult SearchProductByName(string name)
        {
            var matches = _context.Products
                .Where(p => p.ProductName != null && p.ProductName.Contains(name))
                .Select(p => new {
                    id = p.ProductID,
                    productName = p.ProductName,
                    price = p.Price
                })
                .ToList();

            return Json(matches);
        }
    }
}

