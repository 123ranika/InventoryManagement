using System.ComponentModel.DataAnnotations;

namespace InventoryManagement.DataModel
{
    public class InvoiceVM
    {


        public string? CustomerName { get; set; }
        public DateTime Date { get; set; }
        public string? Address { get; set; }
        public string? Phone { get; set; }
        public string? Description { get; set; }
       public float UnitPrice { get; set; }
        public float Discount { get; set; }
        public Guid OrderId { get; set; }
        public List<ProductVM> products { get; set; }
        public float? Subtotal { get; set; }
        public float? GrandTotal { get; set; }
        public float Pay { get; set; }
        public float Due { get; set; }
        public string? PaymentType { get; set; }
        public int Slip { get; set; }
    }

    public class ProductVM
    {
        
        public string? ProductName { get; set; }
        public int Quantity { get; set; }
        public float Price { get; set; }
        public float Total { get; set; }

    }
}
