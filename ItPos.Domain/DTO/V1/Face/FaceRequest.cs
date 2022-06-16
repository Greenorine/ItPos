using System.ComponentModel.DataAnnotations;
using ItPos.Domain.Attributes;

namespace ItPos.Domain.DTO.V1.Face;

public class FaceRequest
{
    public Guid? Id { get; set; }
    public string FullName { get; set; }
    [Birthday]
    public DateTime Birthday { get; set; }
    public string Institute { get; set; }
    public byte[] Photo { get; set; }
    public string Description { get; set; }
    public string VkLink { get; set; }
    public string TgLink { get; set; }
    [EmailAddress]
    public string Email { get; set; }
    [Phone]
    public string Phone { get; set; }
}