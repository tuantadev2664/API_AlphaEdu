using BusinessObjects.Models;
using DataAccessObjects.Dto;
using Repositories.interfaces;
using Services.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.services
{
    public class TeacherAssignmentService : Service<TeacherAssignment>, ITeacherAssignmentService

    {
        private readonly ITeacherAssignmentRepository _repository;

        public TeacherAssignmentService(SchoolDbContext context, ITeacherAssignmentRepository repository)
            : base(context) // Service<T> CRUD chung
        {
            _repository = repository;
        }

        public Task<List<ClassWithStudentCountDto>> GetClassesByTeacherAsync(Guid teacherId, Guid academicYearId)=>_repository.GetClassesByTeacherAsync(teacherId, academicYearId);

        //public Task<List<Class>> GetClassesByTeacherAsync(Guid teacherId, Guid academicYearId) =>
        //    _repository.GetClassesByTeacherAsync(teacherId, academicYearId);

        public Task<List<Subject>> GetSubjectsByTeacherAndClassAsync(Guid teacherId, Guid classId, Guid academicYearId) =>
            _repository.GetSubjectsByTeacherAndClassAsync(teacherId, classId, academicYearId);
    }
}
