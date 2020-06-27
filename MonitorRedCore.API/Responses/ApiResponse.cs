using System;
namespace MonitorRedCore.API.Responses
{
    public class ApiResponse<T>
    {
        public ApiResponse(T data)
        {
            Data = data;
        }

        public T Data { get; set; }
    }
}
