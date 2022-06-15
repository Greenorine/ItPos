using System.ComponentModel.DataAnnotations.Schema;
using ItPos.Domain.Interfaces;

namespace ItPos.Domain.Models;

public class Application : IEntity
{
    public Guid FormId { get; set; }
    public Guid StudentId { get; set; }
    [Column(TypeName = "jsonb")] public ICollection<FormInput> Inputs { get; set; }

    #region IEntity

    public Guid? Id { get; set; }
    public DateTime? CreatedOn { get; set; }
    public DateTime? ModifiedOn { get; set; }
    public string? CreatedBy { get; set; }
    public string? ModifiedBy { get; set; }
    public bool IsDeleted { get; set; }

    #endregion
}
// Делать полный снапшот формы, чтобы сохранить изначальные поля
public class FormInput
{
    public string Name { get; set; }
    public string Value { get; set; }
}