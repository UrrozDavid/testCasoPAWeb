using PAWCP2.Models.TBAModels;

namespace PAWCP2.Models.Entities;

public partial class UserRole : Entity
{
    public int UserId { get; set; }

    public int RoleId { get; set; }

    public string? Description { get; set; }

    public virtual Role Role { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
