using System.ComponentModel.DataAnnotations;

namespace Student_Management.DTOs
{
    public record StudentCreateDto(
        [Required] string Name,
        [Range(0,120)] int Age,
        [EmailAddress] string Email);
}
