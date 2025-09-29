using BusinessObjects.Models;
using Services.interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface IGradeComponentService : IService<GradeComponent>
{
    Task<List<GradeComponent>> GetByClassSubjectTermAsync(Guid classId, Guid subjectId, Guid termId);
    Task<bool> ExistsByNameAsync(Guid classId, Guid subjectId, Guid termId, string name);
}


