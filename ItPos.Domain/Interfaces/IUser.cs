namespace ItPos.Domain.Interfaces;

public interface IUser
{
    public string Name { get; set; }
    public string Login { get; set; }
    public string Password { get; set; }
    public string Roles { get; set; }
    public string? Permissions { get; set; }
    public string Group { get; set; }
}