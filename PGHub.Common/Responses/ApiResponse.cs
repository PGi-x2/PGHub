namespace PGHub.Common.Responses
{
    public class ApiResponse<T>
    {
        public string Message { get; }
        public T Data { get; }
        public int StatusCode { get; }

        private ApiResponse(string message, T data, int statusCode)
        {
            Message = message;
            Data = data;
            StatusCode = statusCode;
        }
        public static ApiResponse<T> SuccesResult(T data, string message, int statusCode = 200)
        {
            return new ApiResponse<T>(message, data, statusCode);
        }
    }
}
