namespace ItPos.Domain.Exceptions;

public class EntityNotFoundException : Exception
{
    public EntityNotFoundException(string field, string? id) : base($"Не найдена сущность с {field} = '{id}'")
    {
    }
}