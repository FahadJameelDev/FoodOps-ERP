using System.ComponentModel.DataAnnotations;

namespace FoodOpsERP.Models.Entities.Manufacturing;

/// <summary>
/// RowVersion only. Used by line/child tables whose audit lives on the header row.
/// </summary>
public abstract class VersionedEntity
{
    [Timestamp]
    public byte[] RowVersion { get; set; } = null!;
}

/// <summary>
/// Audit columns + RowVersion.
/// CreatedAt / CreatedByUserID / ModifiedAt / ModifiedByUserID are stamped automatically.
/// </summary>
public abstract class AuditedEntity : VersionedEntity
{
    public int CreatedByUserID { get; set; }
    public DateTime CreatedAt { get; set; }
    public int? ModifiedByUserID { get; set; }
    public DateTime? ModifiedAt { get; set; }
}