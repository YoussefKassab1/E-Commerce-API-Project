using System.Text.Json.Serialization;

namespace E_Commerce.Common
{
    public enum ResultStatus
    {
        Success,
        NotFound,
        ValidationFailure,
        Failure
    }

    public class GeneralResult
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = string.Empty;
        public ResultStatus Status { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public Dictionary<string, List<string>>? Errors { get; set; }

        public static GeneralResult Success(string message = "Success")
            => new() { IsSuccess = true, Status = ResultStatus.Success, Message = message };

        public static GeneralResult NotFound(string message = "Resource Not Found")
            => new() { IsSuccess = false, Status = ResultStatus.NotFound, Message = message };

        public static GeneralResult Failure(string message = "Operation Failed")
            => new() { IsSuccess = false, Status = ResultStatus.Failure, Message = message };

        public static GeneralResult Failure(Dictionary<string, List<string>> errors, string message = "One or more validation errors occurred")
            => new() { IsSuccess = false, Status = ResultStatus.ValidationFailure, Message = message, Errors = errors };
    }

    public class GeneralResult<T> : GeneralResult
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public T? Data { get; set; }

        public static new GeneralResult<T> Success(T data, string message = "Success")
            => new() { IsSuccess = true, Status = ResultStatus.Success, Message = message, Data = data };

        public static new GeneralResult<T> NotFound(string message = "Resource Not Found")
            => new() { IsSuccess = false, Status = ResultStatus.NotFound, Message = message };

        public static new GeneralResult<T> Failure(string message = "Operation Failed")
            => new() { IsSuccess = false, Status = ResultStatus.Failure, Message = message };

        public static new GeneralResult<T> Failure(Dictionary<string, List<string>> errors, string message = "One or more validation errors occurred")
            => new() { IsSuccess = false, Status = ResultStatus.ValidationFailure, Message = message, Errors = errors };
    }
}
