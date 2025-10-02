using BusinessObjects.Models;
using Services.interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface IAssessmentServices : IService<Assessment>
{
    Task<List<Assessment>> GetByGradeComponentAsync(Guid gradeComponentId);
    Task<List<Assessment>> GetByClassSubjectTermAsync(Guid classId, Guid subjectId, Guid termId);
}



