namespace MonitorRedCore.Core.QueryFilters
{
    #nullable enable
    public class UserQueryFilter
    {
        public string? FirstName { get; set; }
        public string? Email { get; set; }
        public string? Role { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
    }
}
