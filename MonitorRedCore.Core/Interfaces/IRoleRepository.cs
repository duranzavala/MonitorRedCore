﻿using System.Collections.Generic;
using System.Threading.Tasks;
using MonitorRedCore.Core.Models;

namespace MonitorRedCore.Core.Interfaces
{
    public interface IRoleRepository
    {
        IList<Role> GetRoles();
    }
}
