using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class Term
{
    public Guid Id { get; set; }

    public Guid AcademicYearId { get; set; }

    public string Code { get; set; } = null!;

    public DateOnly? StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    public virtual AcademicYear AcademicYear { get; set; } = null!;

    public virtual ICollection<BehaviorNote> BehaviorNotes { get; set; } = new List<BehaviorNote>();

    public virtual ICollection<GradeComponent> GradeComponents { get; set; } = new List<GradeComponent>();
}
