using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class Announcement
{
    public Guid Id { get; set; }

    public Guid SenderId { get; set; }

    public Guid? ClassId { get; set; }

    public Guid? SubjectId { get; set; }

    public string Title { get; set; } = null!;

    public string Content { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public DateTime? ExpiresAt { get; set; }

    public bool? IsUrgent { get; set; }

    public virtual Class? Class { get; set; }

    public virtual User Sender { get; set; } = null!;

    public virtual Subject? Subject { get; set; }
}
