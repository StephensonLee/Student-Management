using System.ComponentModel.DataAnnotations;

namespace Student_Management.DTOs
{
    public record struct CourseCreateDto(
        [Required] string Name,
        [Required] string Teacher);

}
