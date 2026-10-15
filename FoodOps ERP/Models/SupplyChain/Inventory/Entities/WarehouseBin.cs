
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodOpsERP.Models.Entities
{
    public class WarehouseBin
    {
        [Key]
        public int BinID { get; set; }

        public int WarehouseID { get; set; }

        [Required, StringLength(30)]
        public string BinCode { get; set; } = string.Empty;

        [StringLength(30)]
        public string? BinType { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Timestamp]
        public byte[] RowVersion { get; set; } = Array.Empty<byte>();

        // Navigation Properties
        public Warehouse Warehouse { get; set; } = null!;
        public ICollection<InventoryBatch> InventoryBatches { get; set; } = new List<InventoryBatch>();
    }
}