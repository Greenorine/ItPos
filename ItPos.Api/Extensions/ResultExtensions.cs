using ItPos.Domain.Models.Response;
using LanguageExt;
using LanguageExt.Common;
using LanguageExt.SomeHelp;
using Microsoft.AspNetCore.Mvc;

namespace ItPos.Api.Extensions;

public static partial class ResultExtensions
{
    public static IActionResult ToResponse<T>(this Result<T> response) where T : notnull
    {
        return response.Match(res => new ObjectResult(res), e =>
            new BadRequestObjectResult(new ResponseError(new List<string> {e.Message})));
    }
}