using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodOpsERP.Models.Entities.Manufacturing;

public class RecipeItem : VersionedEntity
{
    public int RecipeItemID { get; set; }
    public int RecipeID { get; set; }
    public int LineNo { get; set; }
    public int IngredientProductID { get; set; }

    [Column(TypeName = "decimal(18,4)")]
    public decimal RequiredQty { get; set; }

    public int UOMID { get; set; }

    [Column(TypeName = "decimal(5,2)")]
    public decimal ScrapPercent { get; set; }

    public bool IsCritical { get; set; }

    [MaxLength(200)]
    public string? Remarks { get; set; }

    // Navigation
    public Recipe Recipe { get; set; } = null!;
}