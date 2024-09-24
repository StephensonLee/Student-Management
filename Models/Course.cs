using Student_Management.Models.Domain.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;


namespace Student_Management.Models;

public partial class Course:EntityBase
{
    //public int Id { get; set; }

    [Required(ErrorMessage = "Course name is required.")]
    public string? Name { get; set; }

    [Required(ErrorMessage = "Teacher name is required.")]
    public string? Teacher { get; set; }

    [JsonIgnore]
    public virtual ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
}
