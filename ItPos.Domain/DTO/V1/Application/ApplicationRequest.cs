using ItPos.Domain.Models;

namespace ItPos.Domain.DTO.V1.Application;

public class ApplicationRequest
{
    public Guid FormId { get; set; }
    public Guid StudentId { get; set; }
    public ICollection<FormInput> Inputs { get; set; }
}