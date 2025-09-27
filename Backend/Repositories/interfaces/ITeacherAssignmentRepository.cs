using BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.interfaces
{
    public interface ITeacherAssignmentRepository : IRepository<TeacherAssignment>
    {
        Task<List<Class>> GetClassesByTeacherAsync(Guid teacherId, Guid academicYearId);
        Task<List<Subject>> GetSubjectsByTeacherAndClassAsync(Guid teacherId, Guid classId, Guid academicYearId);
    }
}
