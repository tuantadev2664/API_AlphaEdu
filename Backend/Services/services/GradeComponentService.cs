using BusinessObjects.Models;
using Repositories.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.services
{
    public class GradeComponentService : Service<GradeComponent>, IGradeComponentService
    {
        private readonly IGradeComponentRepository _repository;

        public GradeComponentService(SchoolDbContext context, IGradeComponentRepository repository)
            : base(context)
        {
            _repository = repository;
        }

        public Task<List<GradeComponent>> GetByClassSubjectTermAsync(Guid classId, Guid subjectId, Guid termId)
            => _repository.GetByClassSubjectTermAsync(classId, subjectId, termId);

        public Task<bool> ExistsByNameAsync(Guid classId, Guid subjectId, Guid termId, string name)
            => _repository.ExistsByNameAsync(classId, subjectId, termId, name);
    }
}
