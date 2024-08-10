namespace PGHub.Common.Responses
{
    public class APIResponse<T>
    {
        public string Message { get; }
        public T Data { get; }
        public int StatusCode { get; }
        public bool Success { get; }

        private APIResponse(string message, bool success, T data, int statusCode)
        {
            Message = message;
            Data = data;
            StatusCode = statusCode;
            Success = success;
        }
        public static APIResponse<T> SuccesResult(string message, T data, int statusCode = 200)
        {
            return new APIResponse<T>(message, true, data, statusCode);
        }
        public static APIResponse<T> BadRequest(string message, T data, int statusCode = 400)
        {
            return new APIResponse<T>(message, false, data, statusCode);
        }

        public static APIResponse<T> NotFound(string message, T data, int statusCode = 404)
        {
            return new APIResponse<T>(message, false, data, statusCode);
        }
    }
}
