using System.ComponentModel.DataAnnotations;

namespace Student_Management.DTOs
{
    public record EnrollmentCreateDto(
        [Required] string Name,
        [Required] string Course);
    public record struct EnrollmentGetDto(
        int EnrollmentId,
        string StudentName,
        string? CourseName);
}
