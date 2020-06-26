using MonitorRedCore.Core.Entities;

namespace MonitorRedCore.Core.Models
{
    public partial class Users : BaseEntity
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Role { get; set; }

        public virtual Role RoleNavigation { get; set; }
    }
}
