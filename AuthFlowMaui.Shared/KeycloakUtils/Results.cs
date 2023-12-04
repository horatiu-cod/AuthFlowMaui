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
        public static Result<TData> Success(TData? content) => new(content, true, null, null);
        public static Result<TData> Success(TData? content, HttpStatusCode? HttpStatus) => new(content, true, HttpStatus, null);
        public static Result<TData> Fail(HttpStatusCode HttpStatus) => new(default, false, HttpStatus, null);
        //public static Result<TData> Fail( HttpStatusCode? HttpStatus, TData? content) => new(default, true, HttpStatus, null);
        public static Result<TData> Fail(HttpStatusCode? HttpStatus, string? Error) => new(default, false, HttpStatus, Error);
        public static Result<TData> Fail(string? Error) => new(default, false, null, Error);
    }
}
