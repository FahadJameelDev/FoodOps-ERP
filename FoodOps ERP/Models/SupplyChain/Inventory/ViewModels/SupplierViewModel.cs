using System.ComponentModel.DataAnnotations;

namespace FoodOpsERP.Models.ViewModels.Inventory
{
    public class SupplierViewModel
    {
        public int SupplierID { get; set; }

        [Required(ErrorMessage = "Supplier Code is required")]
        [StringLength(20)]
        public string SupplierCode { get; set; } = string.Empty;

        [Required(ErrorMessage = "Supplier Name is required")]
        [StringLength(150)]
        public string SupplierName { get; set; } = string.Empty;

        [StringLength(100)]
        public string? ContactPerson { get; set; }

        [EmailAddress]
        [StringLength(100)]
        public string? Email { get; set; }

        [StringLength(30)]
        public string? Phone { get; set; }

        [StringLength(300)]
        public string? Address { get; set; }

        [Range(0, 5)]
        public decimal? Rating { get; set; }

        public bool IsActive { get; set; } = true;
    }
}