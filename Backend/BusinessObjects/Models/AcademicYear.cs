using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class AcademicYear
{
    public Guid Id { get; set; }

    public Guid SchoolId { get; set; }

    public string Name { get; set; } = null!;

    public DateOnly? StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    public virtual ICollection<ClassEnrollment> ClassEnrollments { get; set; } = new List<ClassEnrollment>();

    public virtual School School { get; set; } = null!;

    public virtual ICollection<TeacherAssignment> TeacherAssignments { get; set; } = new List<TeacherAssignment>();

    public virtual ICollection<Term> Terms { get; set; } = new List<Term>();
}
