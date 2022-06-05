using System.ComponentModel.DataAnnotations;

namespace ItPos.Domain.DTO;

public class RequestWithId
{
    [Required] public Guid Id { get; set; }
}