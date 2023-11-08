namespace AuthFlowMaui.Shared.Utils
{
#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
    public readonly record struct MethodResult(bool IsSuccess, string? Error)
#pragma warning restore CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
    {
        public static MethodResult Success() => new(true, null);
#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
        public static MethodResult Fail(string? error) => new(false, error);
#pragma warning restore CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
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
#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
        public static MethodDataResult<TData> Fail(string? error,TData? data) => new(false, error, default);
#pragma warning restore CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
    }
}
