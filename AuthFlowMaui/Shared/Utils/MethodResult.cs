namespace AuthFlowMaui.Shared.Utils
{
    public readonly record struct MethodResult(bool IsSuccess, string? Error)
    {
        public static MethodResult Success() => new(true, null);
        public static MethodResult Fail(string? error) => new(false, error);
    }
    public class MethodDataResult<TData>
    {
        public MethodDataResult(bool isSuccess, string error, TData data)
        {
            IsSuccess = isSuccess;
            Error = error;
            Data = data;
        }

        public bool IsSuccess { get; set; }
        public string Error { get; set; }
        public TData Data { get; set; }

        public static MethodDataResult<TData> Success(TData data) => new(true, null, data);
        public static MethodDataResult<TData> Fail(string? error,TData? data) => new(false, error, default);
    }
}
