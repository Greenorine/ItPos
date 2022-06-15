using System.ComponentModel.DataAnnotations.Schema;
using ItPos.Domain.Interfaces;

namespace ItPos.Domain.Models;

public class Form : IEntity
{
    public string Body { get; set; }
    /*[Column(TypeName = "jsonb")] public ICollection<FormInputField> Inputs { get; set; }
    [Column(TypeName = "jsonb")] public ICollection<FormStage> Stages { get; set; }*/

    #region IEntity

    public Guid? Id { get; set; }
    public DateTime? CreatedOn { get; set; }
    public DateTime? ModifiedOn { get; set; }
    public string? CreatedBy { get; set; }
    public string? ModifiedBy { get; set; }
    public bool IsDeleted { get; set; }

    #endregion
}

/*public class FormInputField
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string Type { get; set; }
    public string Options { get; set; }
}*/