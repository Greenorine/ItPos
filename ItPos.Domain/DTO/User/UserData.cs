using System.ComponentModel.DataAnnotations;

namespace ItPos.Domain.DTO.User;

public class UserRequest
{
    public Guid? Id { get; set; }
    [Required]
    public string Login { get; set; }
    [Required]
    public string Password { get; set; }
}