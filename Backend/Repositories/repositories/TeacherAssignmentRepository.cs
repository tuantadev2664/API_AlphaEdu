using BusinessObjects.Models;
using DataAccessObjects;
using Repositories.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.repositories
{
    public class TeacherAssignmentRepository : Repository<TeacherAssignment>, ITeacherAssignmentRepository
    {
        private readonly TeacherAssignmentDAO _dao;

        public TeacherAssignmentRepository(SchoolDbContext context)
            : base(new TeacherAssignmentDAO(context))
        {
            _dao = new TeacherAssignmentDAO(context);
        }

        public Task<List<ClassWithStudentCountDto>> GetClassesByTeacherAsync(Guid teacherId, Guid academicYearId)=>_dao.GetClassesByTeacherAsync(teacherId, academicYearId);

        //public Task<List<Class>> GetClassesByTeacherAsync(Guid teacherId, Guid academicYearId) =>
        //    _dao.GetClassesByTeacherAsync(teacherId, academicYearId);


        public Task<List<Subject>> GetSubjectsByTeacherAndClassAsync(Guid teacherId, Guid classId, Guid academicYearId) =>
            _dao.GetSubjectsByTeacherAndClassAsync(teacherId, classId, academicYearId);
    }
}
