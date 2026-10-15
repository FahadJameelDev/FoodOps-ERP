using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodOpsERP.Models.Entities
{
    public class User
    {
        [Key]
        public int UserID { get; set; }

        public int? PlantID { get; set; }

        [Required, StringLength(50)]
        public string Username { get; set; } = string.Empty;

        [Required, StringLength(100)]
        public string Email { get; set; } = string.Empty;

        [Required, StringLength(255)]
        public string PasswordHash { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Timestamp]
        public byte[] RowVersion { get; set; } = Array.Empty<byte>();

        // Navigation
        public Plant? Plant { get; set; }
    }
}