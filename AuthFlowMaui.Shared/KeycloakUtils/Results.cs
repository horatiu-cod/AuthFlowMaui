using System.Net;

namespace AuthFlowMaui.Shared.KeycloakUtils
{
    public readonly record struct Result(bool IsSuccess, string? Error, HttpStatusCode? HttpStatus)
    {
        public static Result Success() => new(true, null, null);
        public static Result Success(HttpStatusCode? httpStatus) => new(true, null, httpStatus);
        public static Result Fail(string? error) => new(false, error, null);
        public static Result Fail(string? error, HttpStatusCode? httpStatus) => new(false, error, httpStatus);
    }

    public record struct Result<TData>(TData? Content, bool IsSuccess, HttpStatusCode? HttpMessage, string? Error)
    {
        public static Result<TData> Success(TData content) => new(content, true, null, null);
        public static Result<TData> Success(TData content, HttpStatusCode? message) => new(content, true, message, null);
        public static Result<TData> Fail(HttpStatusCode? HttpMessage) => new(default, true, HttpMessage, null);
        public static Result<TData> Fail(TData? content, HttpStatusCode? HttpMessage) => new(default, true, HttpMessage, null);
        public static Result<TData> Fail(TData? content, string? Error) => new(default, true, null, Error);
    }

    public class DataResult<TData>
    {
        public DataResult(bool isSuccess, string? error, TData? data)
        {
            IsSuccess = isSuccess;
            Error = error;
            Data = data;
        }

        public bool IsSuccess { get; set; }
        public string? Error { get; set; }
        public TData? Data { get; set; }

        public static DataResult<TData> Success(TData data) => new(true, null, data);
        public static DataResult<TData> Fail(string? error,TData? data) => new(false, error, default);
    }

    public record HttpStatus(HttpStatusCode StatusCode, string? Message);
}
