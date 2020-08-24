namespace MonitorRedCore.API.Responses
{
    public class GenericResponse<T>
    {
        public bool Success { get; set; }
        public T Data { get; set; }
    }
}
