using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class BehaviorNote
{
    public Guid Id { get; set; }

    public Guid StudentId { get; set; }

    public Guid ClassId { get; set; }

    public Guid TermId { get; set; }

    public string? Note { get; set; }

    public string? Level { get; set; }

    public Guid CreatedBy { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Class Class { get; set; } = null!;

    public virtual User CreatedByNavigation { get; set; } = null!;

    public virtual User Student { get; set; } = null!;

    public virtual Term Term { get; set; } = null!;
}
