using System.Collections.Generic;

namespace MonitorRedCore.Core.Models
{
    public partial class Role
    {
        public Role()
        {
            Users = new HashSet<Users>();
        }

        public int IdRole { get; set; }
        public string RoleType { get; set; }

        public virtual ICollection<Users> Users { get; set; }
    }
}
