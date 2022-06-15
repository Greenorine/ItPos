using System.ComponentModel.DataAnnotations.Schema;
using ItPos.Domain.Models.User;

namespace ItPos.Domain.DTO.V1.StudentInfo;

public class StudentInfoShortResponse
{
    public Guid Id { get; set; }
    public string FullName { get; set; }
    public string AcademicGroup { get; set; }
    public string StudentIdCard { get; set; }
    public string Institute { get; set; }
    public DateTime? ModifiedOn { get; set; }
    public string? ModifiedBy { get; set; }
}