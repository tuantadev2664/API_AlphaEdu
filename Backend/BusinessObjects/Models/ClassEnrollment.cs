using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class ClassEnrollment
{
    public Guid Id { get; set; }

    public Guid ClassId { get; set; }

    public Guid StudentId { get; set; }

    public Guid AcademicYearId { get; set; }

    public virtual AcademicYear AcademicYear { get; set; } = null!;

    public virtual Class Class { get; set; } = null!;

    public virtual User Student { get; set; } = null!;
}
