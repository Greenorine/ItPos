using ItPos.Domain.Interfaces;

namespace ItPos.Domain.Models;

public class Face : IEntity
{
    public string FullName { get; set; }
    public DateTime Birthday { get; set; }
    public string Institute { get; set; }
    public byte[] Photo { get; set; }
    public string Description { get; set; }
    public string VkLink { get; set; }
    public string TgLink { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    
    #region IEntity

    public Guid? Id { get; set; }
    public DateTime? CreatedOn { get; set; }
    public DateTime? ModifiedOn { get; set; }
    public string? CreatedBy { get; set; }
    public string? ModifiedBy { get; set; }
    public bool IsDeleted { get; set; }

    #endregion
}