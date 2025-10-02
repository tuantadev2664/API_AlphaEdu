using BusinessObjects.Models;
using DataAccessObjects;
using Repositories.interfaces;

namespace Repositories.repositories
{
    public class GradeComponentRepository : Repository<GradeComponent>, IGradeComponentRepository
    {
        private readonly GradeComponentDAO _dao;

        public GradeComponentRepository(SchoolDbContext context) : base(new GradeComponentDAO(context))
        {
            _dao = new GradeComponentDAO(context);
        }

        public Task<List<GradeComponent>> GetByClassSubjectTermAsync(Guid classId, Guid subjectId, Guid termId)
            => _dao.GetByClassSubjectTermAsync(classId, subjectId, termId);

        public Task<bool> ExistsByNameAsync(Guid classId, Guid subjectId, Guid termId, string name)
            => _dao.ExistsByNameAsync(classId, subjectId, termId, name);
    }
}



