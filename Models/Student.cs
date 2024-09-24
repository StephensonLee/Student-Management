using Student_Management.Models.Domain.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Student_Management.Models;

public partial class Student : EntityBase
{
    //public int Id { get; set; }

    [Required(ErrorMessage = "Name is required.")]
    [StringLength(50)]
    public string Name { get; set; } = null!;

    [Required(ErrorMessage ="Age is required.")]
    public int Age { get; set; }

    [EmailAddress]
    [Required(ErrorMessage = "Email is required.")]
    public string Email { get; set; } = null!;

    public virtual ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
}
