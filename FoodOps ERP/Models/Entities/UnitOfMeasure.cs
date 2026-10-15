using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodOpsERP.Models.Entities
{
    public class UnitOfMeasure
    {
        [Key]
        public int UOMID { get; set; }

        [Required, StringLength(20)]
        public string UOMCode { get; set; } = string.Empty;

        [Required, StringLength(50)]
        public string UOMName { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Timestamp]
        public byte[] RowVersion { get; set; } = Array.Empty<byte>();
    }
}