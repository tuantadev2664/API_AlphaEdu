using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class Assessment
{
    public Guid Id { get; set; }

    public Guid GradeComponentId { get; set; }

    public string Title { get; set; } = null!;

    public DateOnly? DueDate { get; set; }

    public string? Description { get; set; }

    public virtual GradeComponent GradeComponent { get; set; } = null!;

    public virtual ICollection<Score> Scores { get; set; } = new List<Score>();
}
