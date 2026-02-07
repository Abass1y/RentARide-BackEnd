namespace RentARide.Application.Common
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }
        public List<string>? Errors { get; set; }

        public static ApiResponse<T> SuccessResponse(T? data, string message = "Success")
            => new() { Success = true, Data = data, Message = message };


        public static ApiResponse<T> FailureResponse(string error, string message = "Failed")
          => new()
          {
              Success = false,
              Errors = [error],
              Message = message
          };
       
        public static ApiResponse<T> FailureResponse(List<string> errors, string message = "Failed")
          => new()
          {
              Success = false,
              Errors = errors,
              Message = message
          };
    }
}