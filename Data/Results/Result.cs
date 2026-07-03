using System.Text.Json.Serialization;

namespace Pymex.Payroll.Data.Results
{
    public class Result<T>
    {
        [JsonPropertyName("isSuccess")]
        public bool IsSuccess { get; set; }

        [JsonPropertyName("value")]
        public T? Value { get; set; }

        [JsonPropertyName("errorMessage")]
        public string? ErrorMessage { get; set; }

        public static Result<T> Success(T value) => new() { IsSuccess = true, Value = value };
        public static Result<T> Failure(string message) => new() { IsSuccess = false, ErrorMessage = message };
    }
}
