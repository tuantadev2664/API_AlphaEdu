using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class Class
{
    public Guid Id { get; set; }

    public Guid GradeId { get; set; }

    public string Name { get; set; } = null!;

    public Guid? HomeroomTeacherId { get; set; }

    public virtual ICollection<Announcement> Announcements { get; set; } = new List<Announcement>();

    public virtual ICollection<BehaviorNote> BehaviorNotes { get; set; } = new List<BehaviorNote>();

    public virtual ICollection<ClassEnrollment> ClassEnrollments { get; set; } = new List<ClassEnrollment>();

    public virtual Grade Grade { get; set; } = null!;

    public virtual ICollection<GradeComponent> GradeComponents { get; set; } = new List<GradeComponent>();

    public virtual User? HomeroomTeacher { get; set; }

    public virtual ICollection<TeacherAssignment> TeacherAssignments { get; set; } = new List<TeacherAssignment>();
}
