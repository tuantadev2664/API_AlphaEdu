using BusinessObjects.Models;
using DataAccessObjects;
using DataAccessObjects.Dto;
using Repositories.interfaces;
using Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.repositories
{
    public class SubjectRepository : Repository<Subject>, ISubjectRepository
    {
        private readonly SubjectDAO subject;

        public SubjectRepository(SchoolDbContext context)
            : base(new SubjectDAO(context))
        {
            subject = new SubjectDAO(context);
        }
    
public Task<List<SubjectResultDto>> GetSubjectsByStudentAsync(Guid studentId) => subject.GetSubjectsByStudentAsync(studentId);
    }
}
