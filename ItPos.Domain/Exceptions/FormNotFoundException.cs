namespace ItPos.Domain.Exceptions;

public class FormNotFoundException : Exception
{
    public FormNotFoundException(string formId, Exception? innerException = null) : 
        base($"Форма {formId} не найдена!", innerException)
    {
    }
}