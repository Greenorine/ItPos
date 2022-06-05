namespace ItPos.Domain.Models.Response;

public record ResponseError(IEnumerable<string?> Errors)
{
    public ResponseError(string error) : this(new List<string?> {error})
    {
    }
}