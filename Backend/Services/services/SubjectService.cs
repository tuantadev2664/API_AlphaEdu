using BusinessObjects.Models;
using DataAccessObjects.Dto;
using Repositories.interfaces;
using Repositories.Interfaces;
using Services.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.services
{
    public class SubjectService : Service<Subject>, ISubjectService
    {
        private readonly ISubjectRepository _subject;

        public SubjectService(SchoolDbContext context, ISubjectRepository subjRepository)
            : base(context)
        {
            _subject = subjRepository;
        }

        public Task<List<SubjectResultDto>> GetSubjectsByStudentAsync(Guid studentId) => _subject.GetSubjectsByStudentAsync(studentId);
    }
}
