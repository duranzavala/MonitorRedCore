using System;
using MonitorRedCore.Core.QueryFilters;
using MonitorRedCore.Infraestructure.Interfaces;

namespace MonitorRedCore.Infraestructure.Services
{
    public class UriService : IUriService
    {
        private readonly string _baseUri;

        public UriService(string baseUri)
        {
            _baseUri = baseUri;
        }

        public Uri GetUsersPaginationUri(UserQueryFilter filter, string actionUrl)
        {
            string baseUrl = $"{_baseUri}{actionUrl}";
            return new Uri(baseUrl);
        }
    }
}
