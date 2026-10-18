namespace FoodOpsERP.Models.Entities.Manufacturing;

public class DowntimeLog : AuditedEntity
{
    public int DowntimeID { get; set; }
    public int WorkCenterID { get; set; }
    public int? ProductionOrderID { get; set; }
    public int ReasonID { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public int? DurationMinutes { get; private set; }   // Computed
    public string? Remarks { get; set; }
}