using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class Subject
{
    public Guid Id { get; set; }

    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string Level { get; set; } = null!;

    public bool? IsActive { get; set; }

    public virtual ICollection<Announcement> Announcements { get; set; } = new List<Announcement>();

    public virtual ICollection<GradeComponent> GradeComponents { get; set; } = new List<GradeComponent>();

    public virtual ICollection<TeacherAssignment> TeacherAssignments { get; set; } = new List<TeacherAssignment>();
}
