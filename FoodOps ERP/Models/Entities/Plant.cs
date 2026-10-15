using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodOpsERP.Models.Entities
{
    public class Plant
    {
        [Key]
        public int PlantID { get; set; }

        [Required, StringLength(20)]
        public string PlantCode { get; set; } = string.Empty;

        [Required, StringLength(100)]
        public string PlantName { get; set; } = string.Empty;

        [StringLength(150)]
        public string? Location { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Timestamp]
        public byte[] RowVersion { get; set; } = Array.Empty<byte>();
    }
}