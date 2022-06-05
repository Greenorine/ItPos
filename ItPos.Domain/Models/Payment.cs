using System.ComponentModel.DataAnnotations.Schema;
using ItPos.Domain.Interfaces;

namespace ItPos.Domain.Models;

public class Payment : IEntity
{
    [ForeignKey(nameof(StudentInfo))] public Guid OwnerId { get; set; }
    public DateTime EndDate { get; set; }
    public DateTime PayDate { get; set; }
    public decimal Amount { get; set; }
    public TimeSpan TimeLeft => EndDate - PayDate;
    public string Acquiring { get; set; }
    
    #region IEntity
    public Guid? Id { get; set; }
    public DateTime? CreatedOn { get; set; }
    public DateTime? ModifiedOn { get; set; }
    public string? CreatedBy { get; set; }
    public string? ModifiedBy { get; set; }
    public bool IsDeleted { get; set; }
    #endregion
}