using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class School
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string? District { get; set; }

    public string? City { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<AcademicYear> AcademicYears { get; set; } = new List<AcademicYear>();

    public virtual ICollection<Grade> Grades { get; set; } = new List<Grade>();

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
