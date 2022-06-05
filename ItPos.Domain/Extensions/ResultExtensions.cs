using LanguageExt.Common;

namespace ItPos.Domain.Extensions;

public static partial class ResultExtensions
{
    public static T? ToObjectOrDefault<T>(this Result<T> response) where T : notnull
    {
        return response.Match<T?>(x => x, _ => default);
    }
}