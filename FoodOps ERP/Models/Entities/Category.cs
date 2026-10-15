using System.ComponentModel.DataAnnotations;

namespace FoodOpsERP.Models.Entities
{
    public class Category
    {
        [Key]
        public int CategoryID { get; set; }

        [Required, StringLength(100)]
        public string CategoryName { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Timestamp]
        public byte[] RowVersion { get; set; } = Array.Empty<byte>();

        // Navigation
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}