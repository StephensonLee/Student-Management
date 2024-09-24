using Student_Management.Models.Domain.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Student_Management.Models;

public partial class Enrollment:EntityBase
{
    //public int Id { get; set; }

    [Required(ErrorMessage = "StudentId is required.")]
    public int StudentId { get; set; }

    [Required(ErrorMessage = "CourseId is required.")]
    public int CourseId { get; set; }

    [ForeignKey(nameof(CourseId))]
    public virtual Course Course { get; set; } = null!;

    [ForeignKey(nameof(StudentId))]
    public virtual Student Student { get; set; } = null!;
}
