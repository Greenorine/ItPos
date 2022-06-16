namespace ItPos.Domain.DTO.V1.User;

public class UserRequest
{
    public Guid? Id { get; set; }
    public string Name { get; set; }
    public string Login { get; set; }
    public string Password { get; set; }
    public string Roles { get; set; }
    public string? Permissions { get; set; }
    public string Group { get; set; }
}