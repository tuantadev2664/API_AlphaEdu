using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class ParentStudent
{
    public Guid ParentId { get; set; }

    public Guid StudentId { get; set; }

    public string? Relationship { get; set; }

    public virtual User Parent { get; set; } = null!;

    public virtual User Student { get; set; } = null!;
}
