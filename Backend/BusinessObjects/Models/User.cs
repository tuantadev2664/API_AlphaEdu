using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class User
{
    public Guid Id { get; set; }

    public string Role { get; set; } = null!;

    public string FullName { get; set; } = null!;

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public Guid SchoolId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<Announcement> Announcements { get; set; } = new List<Announcement>();

    public virtual ICollection<BehaviorNote> BehaviorNoteCreatedByNavigations { get; set; } = new List<BehaviorNote>();

    public virtual ICollection<BehaviorNote> BehaviorNoteStudents { get; set; } = new List<BehaviorNote>();

    public virtual ICollection<ClassEnrollment> ClassEnrollments { get; set; } = new List<ClassEnrollment>();

    public virtual ICollection<Class> Classes { get; set; } = new List<Class>();

    public virtual ICollection<Message> MessageReceivers { get; set; } = new List<Message>();

    public virtual ICollection<Message> MessageSenders { get; set; } = new List<Message>();

    public virtual ICollection<ParentStudent> ParentStudentParents { get; set; } = new List<ParentStudent>();

    public virtual ICollection<ParentStudent> ParentStudentStudents { get; set; } = new List<ParentStudent>();

    public virtual School School { get; set; } = null!;

    public virtual ICollection<Score> ScoreCreatedByNavigations { get; set; } = new List<Score>();

    public virtual ICollection<Score> ScoreStudents { get; set; } = new List<Score>();

    public virtual ICollection<TeacherAssignment> TeacherAssignments { get; set; } = new List<TeacherAssignment>();
}
