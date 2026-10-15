
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodOpsERP.Models.Entities
{
    public class PurchaseOrderItem
    {
        [Key]
        public int POItemID { get; set; }

        public int POID { get; set; }
        public int ProductID { get; set; }
        public int UOMID { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        public decimal OrderedQty { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        public decimal UnitCost { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        public decimal ReceivedQty { get; set; } = 0;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Timestamp]
        public byte[] RowVersion { get; set; } = Array.Empty<byte>();

        // Navigation Properties
        public PurchaseOrder PurchaseOrder { get; set; } = null!;
        public Product Product { get; set; } = null!;
        public UnitOfMeasure UOM { get; set; } = null!;
    }
}