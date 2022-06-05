using ItPos.Domain.Interfaces;

namespace ItPos.Domain.Models;

public class Offer : IEntity
{
    public string Title { get; set; }
    public string CompanyName { get; set; }
    public string Description { get; set; }
    public string Activity { get; set; }
    public string DeliveryWay { get; set; }

    #region IEntity
    public Guid? Id { get; set; }
    public DateTime? CreatedOn { get; set; }
    public DateTime? ModifiedOn { get; set; }
    public string? CreatedBy { get; set; }
    public string? ModifiedBy { get; set; }
    public bool IsDeleted { get; set; }
    #endregion
}