using InventoryManagement.Areas.Admin.Data;
using InventoryManagement.Data;
using InventoryManagement.DataModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Security.Claims;


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
        public IActionResult OrderSummarySubmit(InvoiceVM model)
        {
            if (model == null)
            {
                return Json(new { success = false, message = "Invalid invoice data." });
            }
     
            try
            {
                //Guid ClientID;
                if (model.Client != null && model.Client.Phone != null)
                {
                    var anika = _context.Clients.Where(x => x.Phone == model.Client.Phone).FirstOrDefault();
                    if (anika != null) {
                        model.Client.ClientID=anika.ClientID;
                    }
                    else
                    {

                       _context.Clients.Add(model.Client);

                        _context.SaveChanges();
                        model.Client.ClientID = anika.ClientID;
                    }
                }




                Invoice invoiceData = new Invoice();

                //mapping for  InvoiceVM to Invoice datamodel 

                invoiceData.Date = model.Date;
                invoiceData.ClientID = model.Client.ClientID;
                invoiceData.UnitPrice = model.UnitPrice;
                invoiceData.Discount = model.Discount;
                invoiceData.Subtotal = model.Subtotal;
                invoiceData.GrandTotal = model.GrandTotal;
                invoiceData.Pay = model.Pay;
                invoiceData.Due = model.Due;
                invoiceData.PaymentType = model.PaymentType;
                invoiceData.Slip = model.Slip;

                if (invoiceData != null)
                {
                    
                    _context.Invoice.Add(invoiceData);
                    _context.SaveChanges();
                }

                //use foreach loop for inster InvoiceItems
                foreach (var item in model.InvoiceItems)
                {
                    var invoiceItem = new InvoiceItems
                    {
                        InvoiceId = invoiceData.InvoiceId,
                        ProductName = item.ProductName,
                        Quantity = item.Quantity,
                        Price = item.Price,
                        Total = item.Total
                    };
                    _context.InvoiceItems.Add(invoiceItem);
                    _context.SaveChanges();
                }

                return Json(new { success = true, message = "Invoice received successfully." });


            }


            catch (Exception ex)
            {
                return Json(new { success = false, message = "Invoice received Error." });
            }

        }


        [Route("SaleList")]
        public IActionResult SaleList()
        {
            var datalist = _context.Invoice.ToList();

            return View(datalist);
        }
        
    }
}

