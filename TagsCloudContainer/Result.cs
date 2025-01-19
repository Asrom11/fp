namespace TagsCloudContainer;

public class None
{
    private None()
    {
    }
}

public readonly struct Result<TValue>(string error, TValue value = default!)
{
    public TValue Value { get; } = value;
    public string Error { get; } = error;
    public bool IsSuccess => Error == null;

    public static implicit operator Result<TValue>(TValue value) =>
        Result.Ok(value);
}

public static class Result
{
    public static Result<T> AsResult<T>(this T value) =>
        Ok(value);

    public static Result<T> Ok<T>(T value) =>
        new(null!, value);

    public static Result<None> Ok() =>
        Ok<None>(null!);

    public static Result<T> Fail<T>(string error) =>
        new(error);

    public static Result<T> Of<T>(Func<T> func, string? error = null)
    {
        try
        {
            return func();
        }
        catch (Exception e)
        {
            return Fail<T>(error ?? e.Message);
        }
    }

    public static Result<None> OfAction(Action action, string? error = null) =>
        Of<None>(
            () =>
            {
                action();
                return null!;
            },
            error);

    public static Result<None> Then<TInput>(this Result<TInput> result, Action<TInput> action, string? error = null) =>
        result.Then(input => OfAction(() => action(input), error));

    public static Result<TOutput> Then<TInput, TOutput>(this Result<TInput> result, Func<TInput, TOutput> func, string? error = null) =>
        result.Then(input => Of(() => func(input), error));

    public static Result<TOutput> Then<TInput, TOutput>(this Result<TInput> result, Func<TInput, Result<TOutput>> func) =>
        result.IsSuccess ? func(result.Value) : Fail<TOutput>(result.Error);

    public static Result<T> RefineError<T>(this Result<T> result, string error)
        => result.IsSuccess ? result : Fail<T>($"{error}. {result.Error}");
}