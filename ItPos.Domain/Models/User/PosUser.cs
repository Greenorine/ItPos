using ItPos.Domain.Interfaces;

namespace ItPos.Domain.Models.User;

public class PosUser : IEntity
{
    public string Login { get; set; }
    public string Password { get; set; }
    public string Roles { get; set; }
    public string? Permissions { get; set; }
    public string Group { get; set; }

    #region IEntity
    public Guid? Id { get; set; }
    public DateTime? CreatedOn { get; set; }
    public DateTime? ModifiedOn { get; set; }
    public string? CreatedBy { get; set; }
    public string? ModifiedBy { get; set; }
    public bool IsDeleted { get; set; }
    #endregion
}