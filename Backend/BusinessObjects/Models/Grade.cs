using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class Grade
{
    public Guid Id { get; set; }

    public Guid SchoolId { get; set; }

    public string Level { get; set; } = null!;

    public int GradeNumber { get; set; }

    public virtual ICollection<Class> Classes { get; set; } = new List<Class>();

    public virtual School School { get; set; } = null!;
}
