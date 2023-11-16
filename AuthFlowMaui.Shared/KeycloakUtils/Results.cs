namespace AuthFlowMaui.Shared.KeycloakUtils
{
    public readonly record struct Result(bool IsSuccess, string? Error)
    {
        public static Result Success() => new(true, null);
        public static Result Fail(string? error) => new(false, error);
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
}
