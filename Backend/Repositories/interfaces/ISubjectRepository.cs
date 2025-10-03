using BusinessObjects.Models;
using DataAccessObjects.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.interfaces
{
    public interface ISubjectRepository : IRepository<Subject>
    {
        Task<List<SubjectResultDto>> GetSubjectsByStudentAsync(Guid studentId);
    }
}
