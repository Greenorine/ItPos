namespace ItPos.Domain.Exceptions;

public class LoginIsBusyException : Exception
{
    public LoginIsBusyException(string login, Exception? innerException = null) : 
        base($"Логин {login} уже занят!", innerException)
    {
    }
}