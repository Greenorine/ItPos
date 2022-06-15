using System.ComponentModel.DataAnnotations;
using ItPos.Domain.Models.User;

namespace ItPos.Domain.DTO.V1.StudentInfo;

public class StudentInfoRequest
{
    public Guid? Id { get; set; }
    [Required]
    public string FullName { get; set; }
    [Required]
    public Group Institute { get; set; }
    [Required]
    public bool IsContract { get; set; }
    [Required]
    public string AcademicGroup { get; set; }
    [Required]
    public string PosIdCard { get; set; }
    [Required]
    public string StudentIdCard { get; set; }
    [Required]
    public bool HasElectronicSignature { get; set; }
    [Required] [EmailAddress]
    public string Email { get; set; }
    [Required] [Phone]
    public string Phone { get; set; }
    [Required]
    public string Login { get; set; }
    [Required]
    public string Password { get; set; }
}