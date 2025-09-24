using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class Score
{
    public Guid Id { get; set; }

    public Guid AssessmentId { get; set; }

    public Guid StudentId { get; set; }

    public decimal? Score1 { get; set; }

    public bool? IsAbsent { get; set; }

    public string? Comment { get; set; }

    public Guid CreatedBy { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Assessment Assessment { get; set; } = null!;

    public virtual User CreatedByNavigation { get; set; } = null!;

    public virtual User Student { get; set; } = null!;
}
