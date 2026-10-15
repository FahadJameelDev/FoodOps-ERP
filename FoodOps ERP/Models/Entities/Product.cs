using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodOpsERP.Models.Entities
{
    public class Product
    {
        [Key]
        public int ProductID { get; set; }

        public int CategoryID { get; set; }

        [Required, StringLength(30)]
        public string ProductCode { get; set; } = string.Empty;

        [Required, StringLength(150)]
        public string ProductName { get; set; } = string.Empty;

        [StringLength(30)]
        public string? ProductType { get; set; }

        public int BaseUOMID { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        public decimal? MinStockLevel { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Timestamp]
        public byte[] RowVersion { get; set; } = Array.Empty<byte>();

        // Navigation
        public Category Category { get; set; } = null!;
        public UnitOfMeasure BaseUOM { get; set; } = null!;
    }
}