using System;
using MonitorRedCore.Core.QueryFilters;

namespace MonitorRedCore.Infraestructure.Interfaces
{
    public interface IUriService
    {
        Uri GetUsersPaginationUri(UserQueryFilter filter, string actionUrl);
    }
}