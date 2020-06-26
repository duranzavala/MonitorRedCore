using System.Collections.Generic;
using MonitorRedCore.Core.Entities;

namespace MonitorRedCore.Core.Models
{
    public partial class Role : BaseEntity
    {
        public Role()
        {
            Users = new HashSet<Users>();
        }

        public string RoleType { get; set; }

        public virtual ICollection<Users> Users { get; set; }
    }
}
