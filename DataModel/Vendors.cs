using System.ComponentModel.DataAnnotations;

namespace InventoryManagement.DataModel
{
    public class Vendors
    {
        [Key]
        public Guid VendorID { get; set; }
        public string? VendorName { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
    }
}
