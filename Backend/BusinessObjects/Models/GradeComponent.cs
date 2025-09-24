using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class GradeComponent
{
    public Guid Id { get; set; }

    public Guid ClassId { get; set; }

    public Guid SubjectId { get; set; }

    public Guid TermId { get; set; }

    public string Name { get; set; } = null!;

    public string Kind { get; set; } = null!;

    public decimal Weight { get; set; }

    public decimal MaxScore { get; set; }

    public int? Position { get; set; }

    public virtual ICollection<Assessment> Assessments { get; set; } = new List<Assessment>();

    public virtual Class Class { get; set; } = null!;

    public virtual Subject Subject { get; set; } = null!;

    public virtual Term Term { get; set; } = null!;
}
