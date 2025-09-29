using BusinessObjects.Models;
using DataAccessObjects;
using Repositories.interfaces;

namespace Repositories.repositories
{
    public class AssessmentRepository : Repository<Assessment>, IAssessmentRepository
    {
        private readonly AssessmentDAO _dao;

        public AssessmentRepository(SchoolDbContext context) : base(new AssessmentDAO(context))
        {
            _dao = new AssessmentDAO(context);
        }

        public Task<List<Assessment>> GetByGradeComponentAsync(Guid gradeComponentId)
            => _dao.GetByGradeComponentAsync(gradeComponentId);

        public Task<List<Assessment>> GetByClassSubjectTermAsync(Guid classId, Guid subjectId, Guid termId)
            => _dao.GetByClassSubjectTermAsync(classId, subjectId, termId);
    }
}


