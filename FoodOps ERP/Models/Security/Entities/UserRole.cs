using FoodOpsERP.Models.Entities;

namespace FoodOpsERP.Models.Entities.Security;

public class UserRole
{
    public int UserRoleID { get; set; }
    public int UserID { get; set; }
    public int RoleID { get; set; }
    public DateTime AssignedAt { get; set; }
    public int AssignedBy { get; set; }

    public Role Role { get; set; } = null!;
    public User User { get; set; } = null!; // Naya: User navigation
}