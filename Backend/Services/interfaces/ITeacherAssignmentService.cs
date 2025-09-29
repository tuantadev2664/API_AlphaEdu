using BusinessObjects.Models;
using DataAccessObjects.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.interfaces
{
    public interface ITeacherAssignmentService : IService<TeacherAssignment>
    {
        //Task<List<Class>> GetClassesByTeacherAsync(Guid teacherId, Guid academicYearId);
        Task<List<ClassWithStudentCountDto>> GetClassesByTeacherAsync(Guid teacherId, Guid academicYearId);
        Task<List<Subject>> GetSubjectsByTeacherAndClassAsync(Guid teacherId, Guid classId, Guid academicYearId);
    }
}
