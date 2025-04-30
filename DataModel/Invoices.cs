using System.ComponentModel.DataAnnotations;

namespace InventoryManagement.DataModel
{
    public class Invoices
    {
        [Key]
        public Guid InvoiceId { get; set; }

        public string? CustomerName { get; set; }
        public DateTime Date { get; set; }
        public string? Address { get; set; }
        public string? Phone { get; set; }
        public string? Description { get; set; }
        public int Quantity { get; set; }
        public float UnitPrice { get; set; }
        public float Discount { get; set; }
        public float TotalPrice { get; set; }
        public float TotalDiscount { get; set; }
        public float Subtotal { get; set; }
        public float GrandTotal { get; set; }
        public float Due { get; set; }
        public string? PaymentType { get; set; }
        public Guid Slip { get; set; }
    }
}
