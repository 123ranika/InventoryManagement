using System.ComponentModel.DataAnnotations;

namespace InventoryManagement.DataModel
{
    public class Categorys
    {
        [Key]
        public Guid CategoryID { get; set; }
        public string? CategoryName { get; set; }
    }
}
