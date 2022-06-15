using System.ComponentModel.DataAnnotations.Schema;
using ItPos.Domain.Models.User;

namespace ItPos.Domain.DTO.V1.StudentInfo;

public class StudentInfoResponse
{
    public Guid Id { get; set; }
    public string FullName { get; set; }
    public bool IsContract { get; set; }
    public string AcademicGroup { get; set; }
    public string PosIdCard { get; set; }
    public string StudentIdCard { get; set; }
    public bool HasElectronicSignature { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string Login { get; set; }
    public string Password { get; set; }
    public string Institute { get; set; }
    public DateTime? ModifiedOn { get; set; }
    public string? ModifiedBy { get; set; }
}