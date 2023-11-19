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

    public record struct Result<TData>(TData? Content, bool IsSuccess, HttpStatusCode? HttpStatus, string? Error)
    {
        public static Result<TData> Success(TData content) => new(content, true, null, null);
        public static Result<TData> Success(TData content, HttpStatusCode? HttpStatus) => new(content, true, HttpStatus, null);
        public static Result<TData> Fail(HttpStatusCode HttpStatus) => new(default, true, HttpStatus, null);
        public static Result<TData> Fail( HttpStatusCode HttpStatus, TData? content) => new(default, true, HttpStatus, null);
        public static Result<TData> Fail(HttpStatusCode HttpStatus, TData? content, string? Error) => new(default, true, HttpStatus, Error);
        public static Result<TData> Fail(string? Error, TData? content) => new(default, true, null, Error);
    }

    public class DataResult<TData>
    {
        public DataResult(bool isSuccess, string? error,HttpStatusCode? httpStatusCode, TData? data)
        {
            IsSuccess = isSuccess;
            Error = error;
            Data = data;
            HttpStatusCode = httpStatusCode;
        }

        public bool IsSuccess { get; set; }
        public string? Error { get; set; }
        public TData? Data { get; set; }
        public HttpStatusCode? HttpStatusCode { get; set; }

        public static DataResult<TData> Success(TData? data) => new(true, null, null, data);
        public static DataResult<TData> Fail(string? error, TData? data) => new(false, error, null, default);
        public static DataResult<TData> Success(HttpStatusCode? httpStatusCode, TData? data) => new(true, null, httpStatusCode, data);
        public static DataResult<TData> Fail(HttpStatusCode? httpStatusCode, string? error, TData? data) => new(false, error, httpStatusCode, default);
    }

}
