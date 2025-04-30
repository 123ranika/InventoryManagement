using InventoryManagement.Data;
using InventoryManagement.DataModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagement.Areas.Admin.Controllers
{
    [Area("Admin"), Route("Vendor")]
    //[Authorize]
    public class VendorController : Controller
    {

        private readonly ApplicationDbContext _context;

        public VendorController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Route("VendorCreate")]
        public IActionResult VendorCreate()
        {
            return View();
        }

        [HttpPost("VendorCreateSubmit")]
        public IActionResult VendorCreateSubmit(Vendors model)
        {

            if (model.VendorName!= null)
            {

                _context.Vendors.Add(model);
                _context.SaveChanges();

                return RedirectToAction("VendorList");

            }


             return RedirectToAction("VendorCreate");
        }


        [Route("VendorList")]
        public IActionResult VendorList()
        {
            var datalist = _context.Vendors.ToList();


            return View(datalist);
        }
        [Route("DeleteVendor")]
        public IActionResult DeleteVendor(Guid id)
        {
            var vendor = _context.Vendors.FirstOrDefault(v => v.VendorID == id);
            if (vendor != null)
            {
                _context.Vendors.Remove(vendor);
                _context.SaveChanges();
            }

            return RedirectToAction("VendorList");
        }
        [Route("VendorEdit")]
        public IActionResult VendorEdit(Guid id)
        {

            var vendor = _context.Vendors.Where(x => x.VendorID == id).FirstOrDefault();

            if (vendor == null)
            {
                return RedirectToAction("VendorList");
            }
            return View(vendor);
        }
        [HttpPost("VendorUpdateSubmit")]
        public IActionResult VendorUpdateSubmit(Vendors datamodel)
        {
            try
            {
                var vendor = _context.Vendors.Where(x => x.VendorID == datamodel.VendorID).FirstOrDefault();

                if (vendor == null)
                {
                    return RedirectToAction("VendorList");
                }
                vendor.VendorName = datamodel.VendorName;


                _context.Update(vendor);
                _context.SaveChanges();

                return RedirectToAction("VendorList");

            }
            catch
            {
                return RedirectToAction("VendorList");
            }

       
        }

    }
}

