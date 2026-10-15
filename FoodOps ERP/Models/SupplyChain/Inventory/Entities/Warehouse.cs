using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodOpsERP.Models.Entities
{
    public class Warehouse
    {
        [Key]
        public int WarehouseID { get; set; }

        public int PlantID { get; set; }

        [Required, StringLength(20)]
        public string WarehouseCode { get; set; } = string.Empty;

        [Required, StringLength(100)]
        public string WarehouseName { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;

        public int? CreatedByUserID { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public int? ModifiedByUserID { get; set; }
        public DateTime? ModifiedAt { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; } = Array.Empty<byte>();

        // Navigation Properties
        public Plant Plant { get; set; } = null!;
        public ICollection<WarehouseBin> WarehouseBins { get; set; } = new List<WarehouseBin>();
    }
}