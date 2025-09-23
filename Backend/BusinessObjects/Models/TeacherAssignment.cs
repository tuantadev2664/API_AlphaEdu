using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class TeacherAssignment
{
    public Guid Id { get; set; }

    public Guid TeacherId { get; set; }

    public Guid ClassId { get; set; }

    public Guid SubjectId { get; set; }

    public Guid AcademicYearId { get; set; }

    public virtual AcademicYear AcademicYear { get; set; } = null!;

    public virtual Class Class { get; set; } = null!;

    public virtual Subject Subject { get; set; } = null!;

    public virtual User Teacher { get; set; } = null!;
}
