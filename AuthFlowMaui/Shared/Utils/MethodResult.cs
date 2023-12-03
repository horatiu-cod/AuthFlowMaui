namespace AuthFlowMaui.Shared.Utils;
#pragma warning disable
public readonly record struct MethodResult(bool IsSuccess, string? Error)
{
    public static MethodResult Success() => new(true, null);
    public static MethodResult Fail(string? error) => new(false, error);
}

public record struct MethodResult<TData>(TData? Data,bool IsSuccess, string? Error)
{
    public static MethodResult<TData> Success(TData? data) => new(data, true, null);
    public static MethodResult<TData> Fail(string? error) => new(default, false, error);
}