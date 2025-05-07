using System.ComponentModel.DataAnnotations;

namespace InventoryManagement.DataModel
{
    public class Custominfos
    {
        [Key]
        public Guid InvoiceId { get; set; }
        public string? CustomerName { get; set; }
        public DateTime Date { get; set; }
        public string? Address { get; set; }
        public string? Phone { get; set; }

    }
}
