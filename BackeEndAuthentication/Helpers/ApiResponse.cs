namespace BackeEndAuthentication.Helpers
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; } = string.Empty;
        public T Data { get; set; }

        public ApiResponse() { }

        public ApiResponse(int statusCode, string message, T data)
        {
            StatusCode = statusCode;
            Message = message;
            Data = data;
            Success = statusCode >= 200 && statusCode < 300;
        }

        public ApiResponse(int statusCode, string message)
        {
            StatusCode = statusCode;
            Message = message;
            Data = default!;
            Success = statusCode >= 200 && statusCode < 300;
        }
    }
}
