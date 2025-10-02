using BusinessObjects.Models;
using Repositories.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.services
{
    public class AssessmentServices : Service<Assessment>, IAssessmentServices
    {
        private readonly IAssessmentRepository _repository;

        public AssessmentServices(SchoolDbContext context, IAssessmentRepository repository)
            : base(context)
        {
            _repository = repository;
        }

        public Task<List<Assessment>> GetByGradeComponentAsync(Guid gradeComponentId)
            => _repository.GetByGradeComponentAsync(gradeComponentId);

        public Task<List<Assessment>> GetByClassSubjectTermAsync(Guid classId, Guid subjectId, Guid termId)
            => _repository.GetByClassSubjectTermAsync(classId, subjectId, termId);
    }
}
