using System.ComponentModel.DataAnnotations.Schema;
using ItPos.Domain.Interfaces;
using ItPos.Domain.Models.User;

namespace ItPos.Domain.Models;

public class StudentInfo : IEntity
{
    [ForeignKey(nameof(PosUser))] public Guid UserId { get; set; }
    public string FullName { get; set; }
    public bool IsContract { get; set; }
    public string AcademicGroup { get; set; }
    public string PosIdCard { get; set; }
    public string StudentIdCard { get; set; }
    public bool HasElectronicSignature { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }

    #region IEntity
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid? Id { get; set; }
    public DateTime? CreatedOn { get; set; }
    public DateTime? ModifiedOn { get; set; }
    public string? CreatedBy { get; set; }
    public string? ModifiedBy { get; set; }
    public bool IsDeleted { get; set; }
    #endregion
}