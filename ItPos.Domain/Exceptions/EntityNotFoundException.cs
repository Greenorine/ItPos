namespace ItPos.Domain.Exceptions;

public class EntityNotFoundException : Exception
{
    public EntityNotFoundException(string? id) : base($"Не найдена сущность с Id = '{id}'")
    {
        
    }
}