namespace ItPos.Domain.Exceptions;

public class EntityExistsException : Exception
{
    public EntityExistsException() : base("Подобная сущность уже существует")
    {
    }
}